using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using MongoDB.Bson;

namespace Constructor_API.Application.Services
{
    public class BuildingService
    {
        IBuildingRepository _buildingRepository;
        INavigationGroupRepository _navigationGroupRepository;
        IMapper _mapper;

        public BuildingService(IBuildingRepository buildingRepository, IMapper mapper,
            INavigationGroupRepository navigationGroupRepository)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
            _navigationGroupRepository = navigationGroupRepository;
        }

        public async Task<Result.Result> InsertBuilding(CreateBuildingDto buildingDto, string navGroupId,
            CancellationToken cancellationToken)
        {
            var building = _mapper.Map<Building>(buildingDto);
            building.Id = ObjectId.GenerateNewId().ToString();

            var navGroup = await _navigationGroupRepository.FirstAsync(g => g.Id == navGroupId, cancellationToken);
            if (navGroup == null) return Result.Result.Error(new Result.Error("Navigation group not found", 404));
            else 
            {
                navGroup.BuildingIds.Append(building.Id);
                await _navigationGroupRepository.UpdateAsync(g => g.Id == navGroupId, navGroup, cancellationToken);
            }

            await _buildingRepository.AddAsync(building, cancellationToken);
            await _buildingRepository.SaveChanges();
            
            return Result.Result.Success();
        }

        public async Task<Result.Result<Building>> GetBuildingById(string id, CancellationToken cancellationToken)
        {
            var building = await _buildingRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if (building == null) return Result.Result<Building>.Error(new Result.Error("Building not found", 404));

            return Result.Result<Building>.Success(building);
        }

        public async Task<Result.Result<Building>> GetBuildingByName(string name, CancellationToken cancellationToken)
        {
            var building = await _buildingRepository.FirstOrDefaultAsync(b => b.Name == name, cancellationToken);
            if (building == null) return Result.Result<Building>.Error(new Result.Error("Building not found", 404));

            return Result.Result<Building>.Success(building);
        }

        public async Task<Result.Result<IReadOnlyList<Building>>> GetBuildingsByNavGroupId(string navGroupId, CancellationToken cancellationToken)
        {
            var navGroup = await _navigationGroupRepository.FirstOrDefaultAsync(g => g.Id == navGroupId, cancellationToken);
            if (navGroup == null) return Result.Result<IReadOnlyList<Building>>.Error(new Result.Error(
                "Navigation group not found", 404));

            //Building[] buildings = new Building[navGroup.BuildingIds.Length];
            //foreach (var id in navGroup.BuildingIds)
            //{

            //}
            var buildings = await _buildingRepository.ListAsync(b => navGroup.BuildingIds.Contains(b.Id),
                cancellationToken);

            if (buildings == null) return Result.Result<IReadOnlyList<Building>>.Success([]);

            return Result.Result<IReadOnlyList<Building>>.Success(buildings);
        }

        public async Task<Result.Result<IReadOnlyList<Building>>> GetAllBuildings(CancellationToken cancellationToken)
        {
            var buildings = await _buildingRepository.ListAsync(cancellationToken);
            return Result.Result<IReadOnlyList<Building>>.Success(buildings);
        }
    }
}
