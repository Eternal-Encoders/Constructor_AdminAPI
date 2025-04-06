using Constructor_API.Core.Repositories;
using Constructor_API.Helpers;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using Constructor_API.Models.InnerObjects;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace Constructor_API.Application.Services
{
    public class PathService
    {
        readonly BuildingService _buildingService;
        readonly FloorService _floorService;
        readonly GraphPointService _graphPointService;
        readonly FloorConnectionService _floorConnectionService;

        public PathService(FloorService floorService, GraphPointService graphPointService,
            FloorConnectionService floorConnectionService, BuildingService buildingService)
        {
            _floorService = floorService;
            _graphPointService = graphPointService;
            _floorConnectionService = floorConnectionService;
            _buildingService = buildingService;
        }

        public async Task<List<string>?> FindOptimalPath(string startId, string endId)
        {
            var startGP = await _graphPointService.GetGraphPointById(startId, CancellationToken.None);
            var endGP = await _graphPointService.GetGraphPointById(endId, CancellationToken.None);
            var floors = new List<GetFloorDto>();
            var startFloor = await _floorService.GetFloorByIdWithGraphPoints(startGP.FloorId, CancellationToken.None);
            floors.Add(startFloor);
            if (startGP.FloorId != endGP.FloorId)
            {
                var endFloor = await _floorService.GetFloorByIdWithGraphPoints(endGP.FloorId, CancellationToken.None);
                var startBuilding= await _buildingService.GetBuildingById(startFloor.BuildingId,
                    CancellationToken.None);
                if (startBuilding == null)
                {
                    throw new NotFoundException($"Building {startFloor.BuildingId} is not found");
                }
                var endBuilding = await _buildingService.GetBuildingById(endFloor.BuildingId,
                    CancellationToken.None);
                if (endBuilding == null)
                {
                    throw new NotFoundException($"Building {endFloor.BuildingId} is not found");
                }

                if (endBuilding.ProjectId != startBuilding.ProjectId)
                    throw new Exception($"Buildings {startBuilding.Id} and {endBuilding.Id} are from different projects");

                floors.Add(endFloor);

                if (endBuilding.Id != startBuilding.Id)
                {
                    //foreach (var exit in await _buildingService.GetPointsByBuildingAndType(
                    //    startBuilding.Id, "exit", CancellationToken.None))
                    //{

                    //}
                    //foreach (var entrance in await _buildingService.GetPointsByBuildingAndType(
                    //    endBuilding.Id, "exit", CancellationToken.None))
                    //{

                    //}

                    return (await FindPathAStar(startGP, (await _buildingService.
                        GetPointsByBuildingAndType(startBuilding.Id, "exit", CancellationToken.None))[0], floors))
                        .Item1.Concat((await FindPathAStar((await _buildingService.
                        GetPointsByBuildingAndType(endBuilding.Id, "exit", CancellationToken.None))[0], endGP, floors))
                        .Item1).ToList();
                }
            }
            
            return (await FindPathAStar(startGP, endGP, floors)).Item1;
        }

        public async Task<Tuple<List<string>?, double>> FindPathAStar(GraphPoint start, GraphPoint end, List<GetFloorDto> floors)
        {
            var closed = new List<PathNode>();
            var open = new List<PathNode>();

            var connections = await _floorConnectionService.GetConnectionsByBuilding(
                floors.FirstOrDefault(f => f.Id == start.FloorId).BuildingId, CancellationToken.None);

            //Метод для поиска соседей, вложен для возможности изменения floors
            async Task<List<PathNode>> GetNeighbours(PathNode pathNode, GraphPoint goal)
            {
                var result = new List<PathNode>();

                List<GraphPoint> neighbourPoints = [];
                if (pathNode.GraphPoint.ConnectionId != null)
                {
                    if (connections == null)
                        throw new NotFoundException($"Floor connection {pathNode.GraphPoint.ConnectionId} is not found");
                    var connection = connections.FirstOrDefault(c => c.Id == pathNode.GraphPoint.ConnectionId);
                    if (connection == null)
                        throw new NotFoundException($"Floor connection {pathNode.GraphPoint.ConnectionId} is not found");
                    connection.Links ??= [];

                    foreach (var neighbourId in connection.Links)
                    {
                        if (neighbourId == pathNode.CameFrom?.GraphPoint.Id) continue;
                        neighbourPoints.Add(await _graphPointService.GetGraphPointById(neighbourId, CancellationToken.None));
                    }
                }

                foreach (var neighbourId in pathNode.GraphPoint.Links)
                {
                    if (neighbourId == pathNode.CameFrom?.GraphPoint.Id) continue;
                    GraphPoint? neighbour = null;

                    foreach (var floor in floors)
                    {
                        neighbour = floor.GraphPoints?.FirstOrDefault(g => g.Id == neighbourId);
                        if (neighbour != null) break;
                    }
                    if (neighbour == null)
                    {
                        neighbour = await _graphPointService.GetGraphPointById(neighbourId, CancellationToken.None);
                        floors.Add(await _floorService.GetFloorByIdWithGraphPoints(neighbour.FloorId, CancellationToken.None));
                    }
                    neighbourPoints.Add(neighbour);
                }

                foreach (var point in neighbourPoints)
                {
                    // Заполняем данные для точки маршрута.
                    var neighbourNode = new PathNode()
                    {
                        GraphPoint = point,
                        PathLengthFromStart = pathNode.PathLengthFromStart +
                            GetDistanceBetweenNeighbours(pathNode, point),
                        HeuristicPathLength = GetHeuristicPathLength(point, goal, pathNode.GraphPoint),
                        CameFrom = pathNode
                    };
                    result.Add(neighbourNode);
                }
                return result;
            }

            var currentNode = new PathNode
            {
                GraphPoint = start,
                PathLengthFromStart = 0,
                HeuristicPathLength = GetHeuristicPathLength(start, end, null)
            };
            open.Add(currentNode);

            PathNode? openNode;

            while (open.Count > 0)
            {
                currentNode = open.MinBy(n => n.EstimateFullPathLength);

                if (currentNode.GraphPoint.Id == end.Id)
                    return await GetPathForNode(currentNode);

                open.Remove(currentNode);
                closed.Add(currentNode);
                foreach (var neighbourNode in await GetNeighbours(currentNode, end))
                {
                    if (closed.Any(n => n.GraphPoint.Id == neighbourNode.GraphPoint.Id)) continue;

                    openNode = open.FirstOrDefault(n => n.GraphPoint.Id == neighbourNode.GraphPoint.Id);
                    if (openNode == null)
                    {
                        open.Add(neighbourNode);
                    }
                    else
                    {
                        if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                        {
                            openNode.CameFrom = currentNode;
                            openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                        }
                    }
                }
            }
            return new Tuple<List<string>?, double>(null, 0);
        }

        private static double GetDistanceBetweenNeighbours(PathNode from, GraphPoint to)
        {
            return Math.Pow(Math.Pow(from.GraphPoint.X - to.X, 2) + Math.Pow(from.GraphPoint.Y - to.Y, 2), 0.5);
        }

        private static double GetHeuristicPathLength(GraphPoint from, GraphPoint to, GraphPoint? prev/*, Dictionary<string, GetFloorDto> floors*/)
        { 
            if (from.FloorId != null && prev != null && prev.FloorId != from.FloorId) 
                return Math.Pow(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2), 0.5) + 
                (from.FloorId == to.FloorId ? 0 : 1000);

            return Math.Pow(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2), 0.5);
            //Math.Abs(floors[from.FloorId].FloorNumber - floors[to.FloorId].FloorNumber) * 1000;
        }

        private static async Task<Tuple<List<string>, double>?> GetPathForNode(PathNode pathNode)
        {
            var result = new List<string>();
            var currentNode = pathNode;
            while (currentNode != null)
            {
                result.Add(currentNode.GraphPoint.Id);
                currentNode = currentNode.CameFrom;
            }
            result.Reverse();
            return new Tuple<List<string>, double>(result, pathNode.EstimateFullPathLength);
        }
    }
}