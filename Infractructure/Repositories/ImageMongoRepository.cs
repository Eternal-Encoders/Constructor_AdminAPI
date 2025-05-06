using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public class ImageMongoRepository : MongoRepository<Image>, IImageRepository
    {
        public ImageMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
