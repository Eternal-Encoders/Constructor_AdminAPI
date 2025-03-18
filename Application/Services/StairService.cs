using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.Entities;
using System.Threading;

namespace Constructor_API.Application.Services
{
    public class StairService
    {
        IStairRepository _stairRepository;
        IGraphPointRepository _graphPointRepository;
        public StairService(IStairRepository stairRepository, IGraphPointRepository graphPointRepository)
        {
            _stairRepository = stairRepository;
            _graphPointRepository = graphPointRepository;
        }

        //public async Task InsertStair(Stair stair, CancellationToken cancellationToken)
        //{
        //    if (_stairRepository.FirstAsync(s => s.Id == stair.Id, cancellationToken) != null)
        //        throw new AlreadyExistsException($"Stair {stair.Id} already exists");

        //    await _stairRepository.AddAsync(stair, cancellationToken);

        //    //return stair;
        //}

        public async Task<Stair> GetStairById(string id, CancellationToken cancellationToken)
        {
            var stair = await _stairRepository.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (stair == null) throw new NotFoundException("Stair not found");

            return stair;
        }

        public async Task<Stair> GetStairByGraphPoint(string graphPointId, CancellationToken cancellationToken)
        {
            var stair = await _stairRepository.FirstOrDefaultAsync(s => s.Links == null ? false : s.Links.Contains(graphPointId),
                cancellationToken);
            if (stair == null) throw new NotFoundException("Stair not found");

            return stair;
        }

        public async Task<IReadOnlyList<Stair>> GetStairsByBuilding(string buildingId, CancellationToken cancellationToken)
        {
            var stairs = await _stairRepository.ListAsync(s => s.BuildingId == buildingId, cancellationToken);
            if (stairs == null) throw new NotFoundException("Stairs not found");

            return stairs;
        }

        public async Task<IReadOnlyList<Stair>> GetAllStairs(CancellationToken cancellationToken)
        {
            var stairs = await _stairRepository.ListAsync(cancellationToken);

            return stairs;
        }
    }
}
