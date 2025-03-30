using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public sealed class FloorConnectionMongoRepository : MongoRepository<FloorConnection>, IFloorConnectionRepository
    {
        public FloorConnectionMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
