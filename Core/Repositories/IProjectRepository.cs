using Constructor_API.Core.Shared.Storage;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using System.Linq.Expressions;

namespace Constructor_API.Core.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        public Task<GetProjectDto?> FirstGetProjectDtoOrDefaultAsync(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken);
        public Task<GetProjectDto[]> SimpleGetProjectDtoListAsync(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken);
    }
}
