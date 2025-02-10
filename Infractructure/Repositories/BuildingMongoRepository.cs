using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Infractructure.Repositories
{
    public class BuildingMongoRepository : MongoRepository<Building>
    {
        public BuildingMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
