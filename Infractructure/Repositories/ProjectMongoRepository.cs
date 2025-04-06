using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public sealed class ProjectMongoRepository : MongoRepository<Project>, IProjectRepository
    {
        public ProjectMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            
        }
    }
}
