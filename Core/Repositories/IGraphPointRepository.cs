using Constructor_API.Core.Shared.Storage;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;

namespace Constructor_API.Core.Repositories
{
    public interface IGraphPointRepository : IRepository<GraphPoint>
    {
        Task<CreateGraphPointFromFloorDto[]?> CreateGraphPointsFromFloorListAsync(string floorId);
    }
}
