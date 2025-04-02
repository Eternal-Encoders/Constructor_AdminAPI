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
            buildingCollection = dbContext.GetCollection<Building>(typeof(Project).Name);
            floorCollection = dbContext.GetCollection<Floor>(typeof(Project).Name);
            graphPointCollection = dbContext.GetCollection<GraphPoint>(typeof(Project).Name);
            floorConnectionCollection = dbContext.GetCollection<FloorConnection>(typeof(Project).Name);
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
            var buildingUsers = await GetUsersForProject(
                await buildingCollection.Find(b => b.Id == id)
                .Project(b => b.ProjectId)
                .FirstOrDefaultAsync());

            return buildingUsers;
        }

        public async Task<string[]> GetUsersForFloor(string id)
        {
            var floorUsers = await GetUsersForBuilding(
                await floorCollection.Find(f => f.Id == id)
                .Project(f => f.BuildingId)
                .FirstOrDefaultAsync());

            return floorUsers;
        }

        public async Task<string[]> GetUsersForGraphPoint(string id)
        {
            var graphPointUsers = await GetUsersForFloor(
                await graphPointCollection.Find(g => g.Id == id)
                .Project(g => g.FloorId)
                .FirstOrDefaultAsync());

            return graphPointUsers;
        }

        public async Task<string[]> GetUsersForFloorConnection(string id)
        {
            var floorConnectionUsers = await GetUsersForBuilding(
                await floorConnectionCollection.Find(f => f.Id == id)
                .Project(f => f.BuildingId)
                .FirstOrDefaultAsync());

            return floorConnectionUsers;
        }
    }
}
