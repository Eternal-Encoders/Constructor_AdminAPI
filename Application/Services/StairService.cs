using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.Entities;
using System.Threading;

namespace ConstructorAdminAPI.Application.Services
{
    public class StairService
    {
        IStairRepository _stairRepository;
        public StairService(IStairRepository stairRepository)
        {
            _stairRepository = stairRepository;
        }

        public async Task<Result.Result<Stair>> GetStairById(string id, CancellationToken cancellationToken)
        {
            var stair = await _stairRepository.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (stair == null) return Result.Result<Stair>.Error(new Result.Error("Stair not found", 404));

            return Result.Result<Stair>.Success(stair);
        }

        public async Task<Result.Result<Stair>> GetStairByGraphPoint(string graphPointId, CancellationToken cancellationToken)
        {
            var stair = await _stairRepository.FirstOrDefaultAsync(s => s.Links == null ? false : s.Links.Contains(graphPointId),
                cancellationToken);
            if (stair == null) return Result.Result<Stair>.Error(new Result.Error("Stair not found", 404));

            return Result.Result<Stair>.Success(stair);
        }

        public async Task<Result.Result<IReadOnlyList<Stair>>> GetStairsByBuilding(string buildingName, CancellationToken cancellationToken)
        {
            var stairs = await _stairRepository.ListAsync(s => s.Building == buildingName, cancellationToken);
            if (stairs == null) return Result.Result<IReadOnlyList<Stair>>.Error(new Result.Error("Stairs not found", 404));

            return Result.Result<IReadOnlyList<Stair>>.Success(stairs);
        }

        public async Task<Result.Result<IReadOnlyList<Stair>>> GetAllStairs(CancellationToken cancellationToken)
        {
            var stairs = await _stairRepository.ListAsync(cancellationToken);

            return Result.Result<IReadOnlyList<Stair>>.Success(stairs);
        }
    }
}
