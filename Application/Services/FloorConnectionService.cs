using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.Entities;
using System.Threading;

namespace Constructor_API.Application.Services
{
    public class FloorConnectionService
    {
        IFloorConnectionRepository _floorConnectionRepository;
        IGraphPointRepository _graphPointRepository;
        public FloorConnectionService(IFloorConnectionRepository floorConnectionRepository, IGraphPointRepository graphPointRepository)
        {
            _floorConnectionRepository = floorConnectionRepository;
            _graphPointRepository = graphPointRepository;
        }

        //public async Task InsertStair(Stair stair, CancellationToken cancellationToken)
        //{
        //    if (_stairRepository.FirstAsync(s => s.Id == stair.Id, cancellationToken) != null)
        //        throw new AlreadyExistsException($"Stair {stair.Id} already exists");

        //    await _stairRepository.AddAsync(stair, cancellationToken);

        //    //return stair;
        //}

        public async Task<FloorConnection> GetConnectionById(string id, CancellationToken cancellationToken)
        {
            var connection = await _floorConnectionRepository.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (connection == null) throw new NotFoundException("Floors connection is not found");

            return connection;
        }

        public async Task<FloorConnection> GetConnectionByGraphPoint(string graphPointId, CancellationToken cancellationToken)
        {
            var connection = await _floorConnectionRepository.FirstOrDefaultAsync(c => c.Links == null ? false :
                c.Links.Contains(graphPointId), cancellationToken);
            if (connection == null) throw new NotFoundException("Floors connection is not found");

            return connection;
        }

        public async Task<IReadOnlyList<FloorConnection>> GetConnectionsByBuilding(string buildingId, CancellationToken cancellationToken)
        {
            var connections = await _floorConnectionRepository.ListAsync(c => c.BuildingId == buildingId, cancellationToken);
            if (connections == null) throw new NotFoundException("Floors connections are not found");

            return connections;
        }

        public async Task<IReadOnlyList<FloorConnection>> GetAllConnections(CancellationToken cancellationToken)
        {
            var connections = await _floorConnectionRepository.ListAsync(cancellationToken);

            return connections;
        }
    }
}
