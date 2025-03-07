using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public sealed class StairMongoRepository : MongoRepository<Stair>, IStairRepository
    {
        public StairMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
