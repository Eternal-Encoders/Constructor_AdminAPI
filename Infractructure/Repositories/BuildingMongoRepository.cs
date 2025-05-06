using Constructor_API.Core.Repositories;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure.Repositories
{
    public class BuildingMongoRepository : MongoRepository<Building>, IBuildingRepository
    {
        public BuildingMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            
        }

        public async Task<GetBuildingDto[]> SimpleGetBuildingDtoListAsync(Expression<Func<Building, bool>> predicate, CancellationToken cancellationToken)
        {
            var buildings = await DbCollection
                .Find(predicate)
                .Project(b => new GetBuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                })
                .ToListAsync();

            return [.. buildings];
        }

        public async Task<GetBuildingDto?> FirstGetBuildingDtoOrDefaultAsync(Expression<Func<Building, bool>> predicate, CancellationToken cancellationToken)
        {
            var building = await DbCollection
                .Find(predicate)
                .Project(b => new GetBuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    DisplayableName = b.DisplayableName,
                    ProjectId = b.ProjectId,
                    FloorIds = b.FloorIds,
                    Url = b.Url,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    GPS = b.GPS,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    UpdatedBy = b.UpdatedBy,
                })
                .FirstOrDefaultAsync();

            return building;
        }
    }
}
