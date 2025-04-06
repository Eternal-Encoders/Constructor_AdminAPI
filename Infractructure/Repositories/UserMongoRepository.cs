using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class UserMongoRepository : MongoRepository<User>, IUserRepository
    {
        public UserMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
