using Constructor_API.Core.Repositories;
using Constructor_API.Core.Shared;
using Constructor_API.Models.Entities;
using MongoDB.Driver;

namespace Constructor_API.Infractructure.Repositories
{
    public class ProjectUserMongoRepository : MongoRepository<ProjectUser>, IProjectUserRepository
    {
        readonly IMongoCollection<Project> projectCollection;
        readonly IMongoCollection<Building> buildingCollection;
        readonly IMongoCollection<Floor> floorCollection;
        readonly IMongoCollection<GraphPoint> graphPointCollection;
        readonly IMongoCollection<FloorConnection> floorConnectionCollection;

        public ProjectUserMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            projectCollection = dbContext.GetCollection<Project>(typeof(Project).Name);
            buildingCollection = dbContext.GetCollection<Building>(typeof(Building).Name);
            floorCollection = dbContext.GetCollection<Floor>(typeof(Floor).Name);
            graphPointCollection = dbContext.GetCollection<GraphPoint>(typeof(GraphPoint).Name);
            floorConnectionCollection = dbContext.GetCollection<FloorConnection>(typeof(FloorConnection).Name);
        }

        public async Task<string[]> GetUsersForProject(string id)
        {
            var projUserIds = await projectCollection.Find(p => p.Id == id)
                .Project(p => p.ProjectUserIds)
                .FirstOrDefaultAsync();
            var projUsers = await DbCollection.Find(u => projUserIds.Contains(u.Id))
                .Project(u => u.UserId)
                .ToListAsync();

            return [..projUsers];
        }

        public async Task<string[]> GetUsersForBuilding(string id)
        {
            id = await buildingCollection.Find(b => b.Id == id)
                .Project(b => b.ProjectId)
                .FirstOrDefaultAsync();
            var buildingUsers = await GetUsersForProject(id);

            return buildingUsers;
        }

        public async Task<string[]> GetUsersForFloor(string id)
        {
            id = await floorCollection.Find(f => f.Id == id)
                .Project(f => f.BuildingId)
                .FirstOrDefaultAsync();
            var floorUsers = await GetUsersForBuilding(id);

            return floorUsers;
        }

        public async Task<string[]> GetUsersForGraphPoint(string id)
        {
            id = await graphPointCollection.Find(g => g.Id == id)
                .Project(g => g.FloorId)
                .FirstOrDefaultAsync();
            var graphPointUsers = await GetUsersForFloor(id);

            return graphPointUsers;
        }

        public async Task<string[]> GetUsersForFloorConnection(string id)
        {
            id = await floorConnectionCollection.Find(f => f.Id == id)
                .Project(f => f.BuildingId)
                .FirstOrDefaultAsync();
            var floorConnectionUsers = await GetUsersForBuilding(id);

            return floorConnectionUsers;
        }
    }
}
