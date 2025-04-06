using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class IconMongoRepository : MongoRepository<Image>
    {
        public IconMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
