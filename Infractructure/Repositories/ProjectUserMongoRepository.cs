using Constructor_API.Core.Repositories;
using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Infractructure.Repositories
{
    public class ProjectUserMongoRepository : MongoRepository<ProjectUser>, IProjectUserRepository
    {
        readonly IMongoCollection<Project> projectCollection;
        readonly IMongoCollection<Building> buildingCollection;
        readonly IMongoCollection<Floor> floorCollection;
        readonly IMongoCollection<GraphPoint> graphPointCollection;
        readonly IMongoCollection<FloorsTransition> floorsTransitionCollection;

        public ProjectUserMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            projectCollection = dbContext.GetCollection<Project>(typeof(Project).Name);
            buildingCollection = dbContext.GetCollection<Building>(typeof(Building).Name);
            floorCollection = dbContext.GetCollection<Floor>(typeof(Floor).Name);
            graphPointCollection = dbContext.GetCollection<GraphPoint>(typeof(GraphPoint).Name);
            floorsTransitionCollection = dbContext.GetCollection<FloorsTransition>(typeof(FloorsTransition).Name);
        }

        public async Task<string[]> GetUsersForProject(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                throw new ValidationException("Wrong input: specified ID is not a valid 24 digit hex string");
            if (projectCollection.Find(p => p.Id == id).FirstOrDefault() == null)
                throw new NotFoundException($"Project {id} is not found");

            var projUsers = await DbCollection.Find(u => u.ProjectId == id)
                .Project(u => u.UserId)
                .ToListAsync();

            return [..projUsers];
        }

        public async Task<string[]> GetUsersForBuilding(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                throw new ValidationException("Wrong input: specified ID is not a valid 24 digit hex string");
            if (buildingCollection.Find(b => b.Id == id).FirstOrDefault() == null)
                throw new NotFoundException($"Building {id} is not found");

            id = await buildingCollection.Find(b => b.Id == id)
                .Project(b => b.ProjectId)
                .FirstOrDefaultAsync();
            var buildingUsers = await GetUsersForProject(id);

            return buildingUsers;
        }

        public async Task<string[]> GetUsersForFloor(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                throw new ValidationException("Wrong input: specified ID is not a valid 24 digit hex string");
            if (floorCollection.Find(f => f.Id == id).FirstOrDefault() == null)
                throw new NotFoundException($"Floor {id} is not found");

            id = await floorCollection.Find(f => f.Id == id)
                .Project(f => f.BuildingId)
                .FirstOrDefaultAsync();
            var floorUsers = await GetUsersForBuilding(id);

            return floorUsers;
        }

        public async Task<string[]> GetUsersForGraphPoint(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                throw new ValidationException("Wrong input: specified ID is not a valid 24 digit hex string");
            if (graphPointCollection.Find(gp => gp.Id == id).FirstOrDefault() == null)
                throw new NotFoundException($"Graph point {id} is not found");

            id = await graphPointCollection.Find(g => g.Id == id)
                .Project(g => g.FloorId)
                .FirstOrDefaultAsync();
            var graphPointUsers = await GetUsersForFloor(id);

            return graphPointUsers;
        }

        public async Task<string[]> GetUsersForFloorsTransition(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                throw new ValidationException("Wrong input: specified ID is not a valid 24 digit hex string");
            if (floorsTransitionCollection.Find(fc => fc.Id == id).FirstOrDefault() == null)
                throw new NotFoundException($"Floor transition {id} is not found");

            id = await floorsTransitionCollection.Find(f => f.Id == id)
                .Project(f => f.BuildingId)
                .FirstOrDefaultAsync();
            var floorsTransitionUsers = await GetUsersForBuilding(id);

            return floorsTransitionUsers;
        }
    }
}
