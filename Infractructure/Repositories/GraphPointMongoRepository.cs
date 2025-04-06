using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class GraphPointMongoRepository : MongoRepository<GraphPoint>, IGraphPointRepository
    {
        public GraphPointMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
