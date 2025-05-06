using Constructor_API.Core.Repositories;
using Constructor_API.Core.Shared;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure.Repositories
{
    public sealed class ProjectMongoRepository : MongoRepository<Project>, IProjectRepository
    {
        readonly IMongoCollection<ProjectUser> projectUserCollection;
        readonly IMongoCollection<Building> buildingCollection;
        readonly IMongoCollection<Floor> floorCollection;
        readonly IMongoCollection<GraphPoint> graphPointCollection;
        readonly IMongoCollection<FloorsTransition> floorConnectionCollection;
        public ProjectMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            projectUserCollection = dbContext.GetCollection<ProjectUser>(typeof(ProjectUser).Name);
            buildingCollection = dbContext.GetCollection<Building>(typeof(Building).Name);
            floorCollection = dbContext.GetCollection<Floor>(typeof(Floor).Name);
            graphPointCollection = dbContext.GetCollection<GraphPoint>(typeof(GraphPoint).Name);
            floorConnectionCollection = dbContext.GetCollection<FloorsTransition>(typeof(FloorsTransition).Name);
        }

        public async Task<GetProjectDto?> FirstGetProjectDtoOrDefaultAsync(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken)
        {
            var project = await DbCollection
                .Find(predicate)
                .Project(p => new GetProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Url = p.Url,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                })
                .FirstOrDefaultAsync();

            if (project != null)
                project.BuildingNames = [..await buildingCollection
                        .Find(b => b.ProjectId == project.Id)
                        .Project(b => b.Name)
                        .ToListAsync() ?? []];

            return project;
        }

        public async Task<GetProjectDto[]> SimpleGetProjectDtoListAsync(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken)
        {
            var projects = await DbCollection
                .Find(predicate)
                .Project(p => new GetProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                })
                .ToListAsync();

            return [..projects];
        }

        public async Task<Project?> FirstOrDefaultAsyncJoined(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken)
        {
            Project? project = await base.FirstOrDefaultAsync(predicate, cancellationToken);
            if (project == null) return project;
            else
            {
                project.ProjectUsers = [..await projectUserCollection.Find(u => u.ProjectId == project.Id).ToListAsync()];
                return project;
            }
        }

        public async Task<IReadOnlyList<Project>> ListAsyncJoined(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken)
        {
            var projects = await base.ListAsync(predicate, cancellationToken);
            foreach (var project in projects)
            {
                project.ProjectUsers = [.. await projectUserCollection.Find(u => u.ProjectId == project.Id).ToListAsync()];
            }
            return projects;
        }

        public override async Task AddAsync(Project aggregateRoot, CancellationToken cancellationToken)
        {
            ProjectUser[]? projectUsers = aggregateRoot.ProjectUsers;
            aggregateRoot.ProjectUsers = null;
            await base.AddAsync(aggregateRoot, cancellationToken);

            await base.AddCommand(async (IClientSessionHandle s) => await projectUserCollection.InsertManyAsync(projectUsers));
        }

        public override async Task RemoveAsync(Expression<Func<Project, bool>> predicate, CancellationToken cancellationToken)
        {
            var project = await base.FirstOrDefaultAsync(predicate, cancellationToken);
            if (project != null)
            {
                var buildings = await buildingCollection.Find(b => project.BuildingIds.Contains(b.Id)).ToListAsync();
                await base.AddCommand(async (IClientSessionHandle s) => await floorConnectionCollection.DeleteManyAsync(fc =>
                    project.BuildingIds.Contains(fc.BuildingId)));
                foreach (var building in buildings)
                {
                    building.FloorIds ??= [];
                    await base.AddCommand(async (IClientSessionHandle s) => await graphPointCollection.DeleteManyAsync(g => building.FloorIds.Contains(g.FloorId)));
                }
                await base.AddCommand(async (IClientSessionHandle s) => await floorCollection.DeleteManyAsync(f =>
                    project.BuildingIds.Contains(f.BuildingId)));
                await base.AddCommand(async (IClientSessionHandle s) => await buildingCollection.DeleteManyAsync(b =>
                    project.BuildingIds.Contains(b.Id)));
                await base.AddCommand(async (IClientSessionHandle s) => await projectUserCollection.DeleteManyAsync(pu =>
                    pu.ProjectId == project.Id));

                await base.RemoveAsync(predicate, cancellationToken);
            }
        }

        //public override async Task UpdateAsync(Expression<Func<Project, bool>> predicate, Project aggregateRoot, CancellationToken cancellationToken)
        //{
        //    ProjectUser[]? projectUsers = aggregateRoot.ProjectUsers;
        //    aggregateRoot.ProjectUsers = null;
        //    await base.AddAsync(aggregateRoot, cancellationToken);

        //    await base.AddCommand(async () => await projectUserCollection.InsertManyAsync(projectUsers));
        //}
    }
}
