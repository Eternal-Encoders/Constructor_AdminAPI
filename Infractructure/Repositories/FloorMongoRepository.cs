using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class FloorMongoRepository : MongoRepository<Floor>, IFloorRepository
    {
        public FloorMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            
        }
    }
}
