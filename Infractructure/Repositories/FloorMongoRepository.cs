using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Infractructure.Repositories
{
    public class FloorMongoRepository : MongoRepository<Floor>, IFloorRepository
    {
        public FloorMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            
        }
    }
}
