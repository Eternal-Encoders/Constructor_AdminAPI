using Constructor_API.Core.Shared.Storage;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using System.Linq.Expressions;

namespace Constructor_API.Core.Repositories
{
    public interface IBuildingRepository : IRepository<Building>
    {
        public Task<GetBuildingDto[]> SimpleGetBuildingDtoListAsync(Expression<Func<Building, bool>> predicate, CancellationToken cancellationToken);
        public Task<GetBuildingDto?> FirstGetBuildingDtoOrDefaultAsync(Expression<Func<Building, bool>> predicate, CancellationToken cancellationToken);
    }
}
