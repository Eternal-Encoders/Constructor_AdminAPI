using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure.Repositories
{
    public class FloorMongoRepository : MongoRepository<Floor>, IFloorRepository
    {
        public FloorMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
        }

        public async Task<FloorForPathDto[]> FloorForPathDtoListAsync(Expression<Func<Floor, bool>> predicate, CancellationToken cancellationToken)
        {
            List<FloorForPathDto> pathFloors = await DbCollection
                .Find(predicate)
                .Project(f => new FloorForPathDto
                {
                    Id = f.Id,
                    Index = f.Index,
                    GraphPoints = null,
                    BuildingId = f.BuildingId,
                })
                .ToListAsync() ?? [];
            return [.. pathFloors];
        }

        public async Task<GetFloorDto[]> SimpleGetFloorDtoByBuildingListAsync(Expression<Func<Floor, bool>> predicate, CancellationToken cancellationToken)
        {
            List<GetFloorDto> floors = await DbCollection
                .Find(predicate)
                .Project(f => new GetFloorDto
                {
                    Id = f.Id,
                    Index = f.Index,
                    Name = f.Name,
                })
                .ToListAsync() ?? [];

            return [.. floors];
        }
    }
}
