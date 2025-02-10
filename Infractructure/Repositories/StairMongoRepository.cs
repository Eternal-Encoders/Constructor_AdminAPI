using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Infractructure.Repositories
{
    public sealed class StairMongoRepository : MongoRepository<Stair>, IStairRepository
    {
        public StairMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
