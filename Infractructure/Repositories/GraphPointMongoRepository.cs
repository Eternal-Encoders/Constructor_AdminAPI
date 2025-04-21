using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure.Repositories
{
    public class GraphPointMongoRepository : MongoRepository<GraphPoint>, IGraphPointRepository
    {
        public GraphPointMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
        }
    }
}
