using Constructor_API.Core.Repositories;
using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure.Repositories
{
    public class BuildingMongoRepository : MongoRepository<Building>, IBuildingRepository
    {
        readonly IMongoCollection<User> userCollection;
        readonly IMongoCollection<ProjectUser> projectUserCollection;

        public BuildingMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            userCollection = dbContext.GetCollection<User>(typeof(User).Name);
            projectUserCollection = dbContext.GetCollection<ProjectUser>(typeof(ProjectUser).Name);
        }

        public async Task<GetBuildingDto[]> SimpleGetBuildingDtoListAsync(Expression<Func<Building, bool>> predicate, CancellationToken cancellationToken)
        {
            var buildings = await DbCollection
                .Find(predicate)
                .Project(b => new GetBuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Status = b.Status,
                    UpdatedAt = b.UpdatedAt,
                })
                .ToListAsync();

            return [.. buildings];
        }

        public override async Task AddAsync(Building aggregateRoot, CancellationToken cancellationToken)
        {
            var projectUserId = await projectUserCollection.Find(u => u.ProjectId == aggregateRoot.ProjectId).Project(u => u.UserId).FirstOrDefaultAsync();
            if (projectUserId != null)
            {
                var user = await userCollection.Find(u => u.Id == projectUserId).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.LastBuilding = aggregateRoot.Id;
                    await base.AddCommand(async (IClientSessionHandle s) => await userCollection.ReplaceOneAsync(
                        u => u.Id == projectUserId, user));

                    await base.AddAsync(aggregateRoot, cancellationToken);
                }
                else throw new NotFoundException($"User with ID {projectUserId} is not found");
            }
            else throw new NotFoundException($"Project with ID {aggregateRoot.ProjectId} is not found");
        }

        public async Task<GetBuildingDto?> FirstGetBuildingDtoOrDefaultAsync(Expression<Func<Building, bool>> predicate, CancellationToken cancellationToken)
        {
            var building = await DbCollection
                .Find(predicate)
                .Project(b => new GetBuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Status = b.Status,
                    DisplayableName = b.DisplayableName,
                    ProjectId = b.ProjectId,
                    //FloorIds = b.FloorIds,
                    ImageId = b.ImageId,
                    Url = b.Url,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    GPS = b.GPS,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    UpdatedBy = b.UpdatedBy,
                    LastFloorId = b.LastFloorId,
                })
                .FirstOrDefaultAsync();

            var projectUserId = await projectUserCollection.Find(u => u.ProjectId == building.ProjectId).Project(u => u.UserId).FirstOrDefaultAsync();
            if (projectUserId != null)
            {
                var user = await userCollection.Find(u => u.Id == projectUserId).FirstOrDefaultAsync();
                if (user != null) 
                {
                    user.LastBuilding = building.Id;
                    await base.AddCommand(async (IClientSessionHandle s) => await userCollection.ReplaceOneAsync(
                        u => u.Id == projectUserId, user));
                }
            }
            else return null;

            return building;
        }

        public override async Task UpdateAsync(Expression<Func<Building, bool>> predicate, Building aggregateRoot, CancellationToken cancellationToken)
        {
            var projectUserId = await projectUserCollection.Find(u => u.ProjectId == aggregateRoot.ProjectId).Project(u => u.UserId).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Project with ID {aggregateRoot.ProjectId} is not found");

            var user = await userCollection.Find(u => u.Id == projectUserId).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"User with ID {projectUserId} is not found");

            user.LastBuilding = aggregateRoot.Id;
            await base.AddCommand(async (IClientSessionHandle s) => await userCollection.ReplaceOneAsync(
                u => u.Id == projectUserId, user));

            await base.UpdateAsync(predicate, aggregateRoot, cancellationToken);   
        }

        public override async Task RemoveAsync(Expression<Func<Building, bool>> predicate, CancellationToken cancellationToken)
        {
            var buildingId = await DbCollection.Find(predicate).Project(b => b.Id).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Building is not found");

            var projectId = await DbCollection.Find(predicate).Project(b => b.ProjectId).FirstOrDefaultAsync();

            var projectUserId = await projectUserCollection.Find(u => u.ProjectId == projectId).Project(u => u.UserId).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Project with ID {projectId} is not found");

            var user = await userCollection.Find(u => u.Id == projectUserId).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"User with ID {projectUserId} is not found");

            if (user.LastBuilding == buildingId)
            {
                user.LastBuilding = "";
                await base.AddCommand(async (IClientSessionHandle s) => await userCollection.ReplaceOneAsync(
                    u => u.Id == projectUserId, user));
            }
            await base.RemoveAsync(predicate, cancellationToken);
        }
    }
}
