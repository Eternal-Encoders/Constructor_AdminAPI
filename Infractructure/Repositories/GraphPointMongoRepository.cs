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

        public async Task<CreateGraphPointFromFloorDto[]?> CreateGraphPointsFromFloorListAsync(string floorId)
        {

            return [.. await DbCollection
                .Find(gp => gp.FloorId == floorId)
                .Project(gp => new CreateGraphPointFromFloorDto
                {
                    Id = gp.Id,
                    X = gp.X,
                    Y = gp.Y,
                    Links = gp.Links,
                    Types = gp.Types,
                    Name = gp.Name,
                    Synonyms = gp.Synonyms,
                    Time = gp.Time,
                    Description = gp.Description,
                    Info = gp.Info,
                    RouteActive = gp.RouteActive,
                    TransitionId = gp.TransitionId
                })
                .ToListAsync()];
        }
    }
}
