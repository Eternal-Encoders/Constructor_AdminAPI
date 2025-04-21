using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure.Repositories
{
    public class UserMongoRepository : MongoRepository<User>, IUserRepository
    {
        readonly IMongoCollection<Project> projectCollection;
        readonly IMongoCollection<ProjectUser> projectUserCollection;
        readonly IMongoCollection<Building> buildingCollection;
        readonly IMongoCollection<Floor> floorCollection;
        readonly IMongoCollection<GraphPoint> graphPointCollection;
        readonly IMongoCollection<FloorsTransition> floorConnectionCollection;

        public UserMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            projectCollection = dbContext.GetCollection<Project>(typeof(Project).Name);
            projectUserCollection = dbContext.GetCollection<ProjectUser>(typeof(ProjectUser).Name);
            buildingCollection = dbContext.GetCollection<Building>(typeof(Building).Name);
            floorCollection = dbContext.GetCollection<Floor>(typeof(Floor).Name);
            graphPointCollection = dbContext.GetCollection<GraphPoint>(typeof(GraphPoint).Name);
            floorConnectionCollection = dbContext.GetCollection<FloorsTransition>(typeof(FloorsTransition).Name);
        }

        public override async Task RemoveAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
        {
            var user = await base.FirstOrDefaultAsync(predicate, cancellationToken);
            if (user != null)
            {
                var projectIds = await projectUserCollection.Find(pu => pu.UserId == user.Id).Project(pu => pu.ProjectId).ToListAsync();
                var buildings = await buildingCollection.Find(b => projectIds.Contains(b.ProjectId)).ToListAsync();
                foreach (var building in buildings)
                {
                    await base.AddCommand(async () => await floorConnectionCollection.DeleteManyAsync(fc => fc.BuildingId == building.Id));
                    building.FloorIds ??= [];
                    await base.AddCommand(async () => await graphPointCollection.DeleteManyAsync(g => building.FloorIds.Contains(g.FloorId)));
                    await base.AddCommand(async () => await floorCollection.DeleteManyAsync(f => f.BuildingId == building.Id));
                }

                await base.AddCommand(async () => await buildingCollection.DeleteManyAsync(b =>
                    projectIds.Contains(b.ProjectId)));
                await base.AddCommand(async () => await projectCollection.DeleteManyAsync(p =>
                    projectIds.Contains(p.Id)));

                await base.RemoveAsync(predicate, cancellationToken);
            }
        }
    }
}
