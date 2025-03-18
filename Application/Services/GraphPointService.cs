using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;

namespace Constructor_API.Application.Services
{
    public class GraphPointService
    {
        IGraphPointRepository _graphPointRepository;
        IStairRepository _stairRepository;
        IFloorRepository _floorRepository;

        public GraphPointService(IGraphPointRepository graphPointRepository, IStairRepository stairRepository,
            IFloorRepository floorRepository)
        {
            _graphPointRepository = graphPointRepository;
            _stairRepository = stairRepository;
            _floorRepository = floorRepository;
        }

        public async Task InsertGraphPoint(GraphPoint graphPoint, CancellationToken cancellationToken)
        {
            if (await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == graphPoint.Id, cancellationToken) != null)
                throw new AlreadyExistsException($"Graph point {graphPoint.Id} already exists");

            Floor floor = await _floorRepository.FirstOrDefaultAsync(f => f.Id == graphPoint.FloorId, cancellationToken);

            if (floor == null)
                throw new NotFoundException($"Floor {graphPoint.FloorId} is not found");

            floor.GraphPoints.Append(graphPoint.StairId);
            await _floorRepository.UpdateAsync(f => f.Id == graphPoint.FloorId, floor, cancellationToken);

            if (graphPoint.StairId != null)
            {
                Stair? stair = await _stairRepository.FirstOrDefaultAsync(s => s.Id == graphPoint.StairId,
                    cancellationToken);
                if (stair == null)
                {
                    var newStair = new Stair
                    {
                        Id = graphPoint.StairId,
                        BuildingId = floor.BuildingId,
                        Links = [],
                    };
                    await _stairRepository.AddAsync(newStair, cancellationToken);
                }
                else
                {
                    stair.Links = stair.Links == null ? [] : stair.Links;

                    stair.Links.Append(graphPoint.Id);
                    await _stairRepository.UpdateAsync(s => s.Id == stair.Id, stair, cancellationToken);
                }
            }

            await _graphPointRepository.AddAsync(graphPoint, cancellationToken);

            await _graphPointRepository.SaveChanges();

            //return Result.Result.Success();
        }

        public async Task InsertGraphPoints(GraphPoint[] graphPoints, CancellationToken cancellationToken)
        {
            foreach (GraphPoint graphPoint in graphPoints)
            {
                Floor floor = await _floorRepository.FirstOrDefaultAsync(f => f.Id == graphPoint.FloorId, cancellationToken);

                if (floor == null)
                    throw new NotFoundException($"Floor {graphPoint.FloorId} is not found");

                floor.GraphPoints.Append(graphPoint.StairId);
                await _floorRepository.UpdateAsync(f => f.Id == graphPoint.FloorId, floor, cancellationToken);

                if (graphPoint.StairId != null)
                {
                    Stair? stair = await _stairRepository.FirstOrDefaultAsync(z => z.Id == graphPoint.StairId, cancellationToken);

                    if (stair == null)
                    {
                        var newStair = new Stair
                        {
                            Id = graphPoint.StairId,
                            BuildingId = floor.BuildingId,
                            Links = [],
                        };
                        await _stairRepository.AddAsync(newStair, cancellationToken);
                    }
                    else
                    {
                        stair.Links = stair.Links == null ? [] : stair.Links;

                        stair.Links.Append(graphPoint.Id);
                        await _stairRepository.UpdateAsync(s => s.Id == stair.Id, stair, cancellationToken);
                    }
                }
            }

            await _graphPointRepository.AddRangeAsync(graphPoints, cancellationToken);

            await _graphPointRepository.SaveChanges();
        }

        public async Task<GraphPoint> GetGraphPointById(string id, CancellationToken cancellationToken)
        {
            var graphPoint = await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (graphPoint == null) throw new NotFoundException($"Graph point not found");

            return graphPoint;
        }

        public async Task<IReadOnlyList<GraphPoint>> GetAllGraphPoints(CancellationToken cancellationToken)
        {
            var graphPoints = await _graphPointRepository.ListAsync(cancellationToken);

            return graphPoints;
        }

        public async Task<Stair> GetStairByGraphPoint(string id, CancellationToken cancellationToken)
        {
            var stair = await _stairRepository.FirstOrDefaultAsync(s => s.Links == null ? false : s.Links.Contains(id),
                cancellationToken);
            if (stair == null) throw new NotFoundException("Stair not found");

            return stair;
        }
    }
}
