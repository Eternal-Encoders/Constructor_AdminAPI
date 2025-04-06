using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class PredefinedCategoryMongoRepository : MongoRepository<PredefinedCategory>, IPredefinedCategoryRepository
    {
        public PredefinedCategoryMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            //ReadOnly = true;
        }
    }
}
