using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public sealed class NavigationGroupMongoRepository : MongoRepository<NavigationGroup>, INavigationGroupRepository
    {
        public NavigationGroupMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            
        }
    }
}
