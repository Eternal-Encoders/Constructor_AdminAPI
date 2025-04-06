using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class BuildingMongoRepository : MongoRepository<Building>, IBuildingRepository
    {
        public BuildingMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
