using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using Minio.DataModel.ILM;
using MongoDB.Bson;
using System.Threading;

namespace Constructor_API.Application.Services
{
    public class FloorsTransitionService
    {
        IFloorsTransitionRepository _floorsTransitionRepository;
        IMapper _mapper;
        IBuildingRepository _buildingRepository;
        //IGraphPointRepository _graphPointRepository;
        public FloorsTransitionService(IFloorsTransitionRepository floorsTransitionRepository/*, IGraphPointRepository graphPointRepository*/,
            IMapper mapper,
            IBuildingRepository buildingRepository)
        {
            _floorsTransitionRepository = floorsTransitionRepository;
            //_graphPointRepository = graphPointRepository;
            _mapper = mapper;
            _buildingRepository = buildingRepository;
        }

        public async Task<FloorsTransition> InsertTransition(CreateFloorsTransitionDto transitionDto, CancellationToken cancellationToken)
        {
            var floorsTransition = _mapper.Map<FloorsTransition>(transitionDto);
            floorsTransition.LinkIds = [];
            floorsTransition.Id = ObjectId.GenerateNewId().ToString();

            if (await _buildingRepository.CountAsync(b => b.Id == floorsTransition.BuildingId, cancellationToken) == 0)
                throw new NotFoundException("Building is not found");

            if (await _floorsTransitionRepository.CountAsync(ft => ft.BuildingId == floorsTransition.BuildingId
                && ft.Name == floorsTransition.Name, cancellationToken) != 0)
                throw new AlreadyExistsException(
                    $"Floors Transition {floorsTransition.Name} already exists in building {floorsTransition.BuildingId}");

            await _floorsTransitionRepository.AddAsync(floorsTransition, cancellationToken);
            await _floorsTransitionRepository.SaveChanges();

            return floorsTransition;
        }

        public async Task<FloorsTransition> GetTransitionById(string id, CancellationToken cancellationToken)
        {
            return await _floorsTransitionRepository.FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
                ?? throw new NotFoundException("Floors transition is not found");
        }

        public async Task<FloorsTransition> GetTransitionByGraphPoint(string graphPointId, CancellationToken cancellationToken)
        {
            return await _floorsTransitionRepository.FirstOrDefaultAsync(c => 
                c.LinkIds == null ? false : c.LinkIds.Contains(graphPointId), cancellationToken) 
                ?? throw new NotFoundException("Floors transition is not found");
        }

        public async Task<IReadOnlyList<FloorsTransition>> GetTransitionsByBuilding(string buildingId, CancellationToken cancellationToken)
        {
            return await _floorsTransitionRepository.ListAsync(c => 
                c.BuildingId == buildingId, cancellationToken)
                ?? throw new NotFoundException("Floors transitions are not found");
        }

        public async Task<IReadOnlyList<FloorsTransition>> GetAllTransitions(CancellationToken cancellationToken)
        {
            return await _floorsTransitionRepository.ListAsync(cancellationToken);
        }

        public async Task DeleteTransition(string transitionId, CancellationToken cancellationToken)
        {
            await _floorsTransitionRepository.RemoveAsync(x => x.Id == transitionId, cancellationToken);
            await _floorsTransitionRepository.SaveChanges();
        }
    }
}
