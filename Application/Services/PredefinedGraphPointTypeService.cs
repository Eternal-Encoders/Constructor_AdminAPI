using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.Entities;

namespace Constructor_API.Application.Services
{
    public class PredefinedGraphPointTypeService
    {
        IPredefinedGraphPointTypeRepository _repository;

        public PredefinedGraphPointTypeService(IPredefinedGraphPointTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<PredefinedGraphPointType>> GetPredefinedTypes(CancellationToken cancellationToken)
        {
            var res = await _repository.ListAsync(cancellationToken);

            return res;
        }
    }
}
