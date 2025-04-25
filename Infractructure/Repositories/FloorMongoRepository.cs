using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using MongoDB.Driver;

namespace Constructor_API.Infractructure.Repositories
{
    public class FloorMongoRepository : MongoRepository<Floor>, IFloorRepository
    {
        readonly IMongoCollection<GraphPoint> graphPointCollection;

        public FloorMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            graphPointCollection = dbContext.GetCollection<GraphPoint>(typeof(GraphPoint).Name);
        }

        public async Task<CreateGraphPointFromFloorDto[]?> GraphPointsFromFloorListAsync(string floorId)
        {
            var graphPoints = await graphPointCollection
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
                .ToListAsync();

            return [.. graphPoints];
        }
    }
}
