using Constructor_API.Core.Shared.Storage;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using System.Linq.Expressions;

namespace Constructor_API.Core.Repositories
{
    public interface IFloorRepository : IRepository<Floor>
    {
        Task<GetFloorDto[]> SimpleGetFloorDtoByBuildingListAsync(Expression<Func<Floor, bool>> predicate, CancellationToken cancellationToken);
        Task<FloorForPathDto[]> FloorForPathDtoListAsync(Expression<Func<Floor, bool>> predicate, CancellationToken cancellationToken);
    }
}
