using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class IconMongoRepository : MongoRepository<Icon>, IIconRepository
    {
        public IconMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
