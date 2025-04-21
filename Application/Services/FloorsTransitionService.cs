using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.Entities;
using System.Threading;

namespace Constructor_API.Application.Services
{
    public class FloorsTransitionService
    {
        IFloorsTransitionRepository _floorsTransitionRepository;
        //IGraphPointRepository _graphPointRepository;
        public FloorsTransitionService(IFloorsTransitionRepository floorsTransitionRepository/*, IGraphPointRepository graphPointRepository*/)
        {
            _floorsTransitionRepository = floorsTransitionRepository;
            //_graphPointRepository = graphPointRepository;
        }

        public async Task<FloorsTransition> GetTransitionById(string id, CancellationToken cancellationToken)
        {
            var transition = await _floorsTransitionRepository.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (transition == null) throw new NotFoundException("Floors transition is not found");

            return transition;
        }

        public async Task<FloorsTransition> GetTransitionByGraphPoint(string graphPointId, CancellationToken cancellationToken)
        {
            var transition = await _floorsTransitionRepository.FirstOrDefaultAsync(c => c.LinkIds == null ? false :
                c.LinkIds.Contains(graphPointId), cancellationToken);
            if (transition == null) throw new NotFoundException("Floors transition is not found");

            return transition;
        }

        public async Task<IReadOnlyList<FloorsTransition>> GetTransitionsByBuilding(string buildingId, CancellationToken cancellationToken)
        {
            var transitions = await _floorsTransitionRepository.ListAsync(c => c.BuildingId == buildingId, cancellationToken);
            if (transitions == null) throw new NotFoundException("Floors transitions are not found");

            return transitions;
        }

        public async Task<IReadOnlyList<FloorsTransition>> GetAllTransitions(CancellationToken cancellationToken)
        {
            var transitions = await _floorsTransitionRepository.ListAsync(cancellationToken);

            return transitions;
        }
    }
}
