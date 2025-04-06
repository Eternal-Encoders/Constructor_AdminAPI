using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class PredefinedGraphPointTypeMongoRepository : MongoRepository<PredefinedGraphPointType>,
        IPredefinedGraphPointTypeRepository
    {
        public PredefinedGraphPointTypeMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            //ReadOnly = true;
        }
    }
}
