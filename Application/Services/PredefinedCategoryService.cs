using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Application.Services
{
    public class PredefinedCategoryService
    {
        IPredefinedCategoryRepository _repository;
        public PredefinedCategoryService(IPredefinedCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<PredefinedCategory>> GetPredefinedCategories(CancellationToken cancellationToken)
        {
            var res = await _repository.ListAsync(cancellationToken);

            return res;
        }
    }
}
