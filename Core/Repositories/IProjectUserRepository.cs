using Constructor_API.Core.Shared.Storage;
using Constructor_API.Models.Entities;

namespace Constructor_API.Core.Repositories
{
    public interface IProjectUserRepository : IRepository<ProjectUser>
    {
        Task<string[]> GetUsersForProject(string id);
        Task<string[]> GetUsersForBuilding(string id);
        Task<string[]> GetUsersForFloor(string id);
        Task<string[]> GetUsersForGraphPoint(string id);
        Task<string[]> GetUsersForFloorConnection(string id);
    }
}
