using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Infractructure.Repositories
{
    public class IconMongoRepository : MongoRepository<Icon>
    {
        public IconMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
