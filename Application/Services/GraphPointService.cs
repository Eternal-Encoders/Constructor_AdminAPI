using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Application.Services
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

        public async Task<Result.Result> InsertGraphPoint(GraphPoint graphPoint, CancellationToken cancellationToken)
        {
            if (await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == graphPoint.Id, cancellationToken) != null)
                return Result.Result.Error(new Result.Error($"Graph point {graphPoint.Id} already exists", 400));

            if (await _floorRepository.FirstOrDefaultAsync(f => f.Building == graphPoint.Building &&
                f.FloorNumber == graphPoint.Floor, cancellationToken) == null)
                return Result.Result.Error(new Result.Error($"Floor not found", 404));

            if (graphPoint.StairId != null)
            {
                Stair? stair = await _stairRepository.FirstOrDefaultAsync(z => z.Id == graphPoint.StairId, cancellationToken);
                if (stair == null)
                {
                    var newStair = new Stair
                    {
                        Id = graphPoint.StairId,
                        Building = graphPoint.Building,
                        StairPoint = graphPoint.StairId,
                        Links = [],
                    };
                    await _stairRepository.AddAsync(newStair, cancellationToken);
                }
                else
                {
                    stair.Links = stair.Links == null ? [] : stair.Links;

                    stair.Links.Append(graphPoint.Id);
                    await _stairRepository.UpdateAsync(stair.Id, stair, cancellationToken);
                }
            }

            await _graphPointRepository.AddAsync(graphPoint, cancellationToken);

            await _graphPointRepository.SaveChanges();

            return Result.Result.Success();
        }

        public async Task<Result.Result<GraphPoint>> GetGraphPointById(string id, CancellationToken cancellationToken)
        {
            var graphPoint = await _graphPointRepository.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (graphPoint == null) return Result.Result<GraphPoint>.Error(new Result.Error("Graph point not found", 404));

            return Result.Result<GraphPoint>.Success(graphPoint);
        }

        public async Task<Result.Result<IReadOnlyList<GraphPoint>>> GetGraphPointsByBuilding(string buildingName, CancellationToken cancellationToken)
        {
            var graphPoints = await _graphPointRepository.ListAsync(g => g.Building == buildingName, cancellationToken);
            if (graphPoints == null) return Result.Result<IReadOnlyList<GraphPoint>>.Error(
                new Result.Error("Graph points not found", 404));

            return Result.Result<IReadOnlyList<GraphPoint>>.Success(graphPoints);
        }

        public async Task<Result.Result<IReadOnlyList<GraphPoint>>> GetGraphPointsByFloor(
            string buildingName, int floorNum, CancellationToken cancellationToken)
        {
            var graphPoints = await _graphPointRepository.ListAsync(g => 
                g.Building == buildingName && g.Floor == floorNum, cancellationToken);
            if (graphPoints == null) return Result.Result<IReadOnlyList<GraphPoint>>.Error(
                new Result.Error("Graph points not found", 404));

            return Result.Result<IReadOnlyList<GraphPoint>>.Success(graphPoints);
        }

        public async Task<Result.Result<IReadOnlyList<GraphPoint>>> GetAllGraphPoints(CancellationToken cancellationToken)
        {
            var graphPoints = await _graphPointRepository.ListAsync(cancellationToken);

            return Result.Result<IReadOnlyList<GraphPoint>>.Success(graphPoints);
        }
    }
}
