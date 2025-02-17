using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Infractructure.Repositories
{
    public class BuildingMongoRepository : MongoRepository<Building>, IBuildingRepository
    {
        public BuildingMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
