using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Infractructure.Repositories
{
    public class GraphPointMongoRepository : MongoRepository<GraphPoint>, IGraphPointRepository
    {
        public GraphPointMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
