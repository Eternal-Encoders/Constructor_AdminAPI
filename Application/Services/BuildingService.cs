using AutoMapper;
using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Models.DTOs;
using ConstructorAdminAPI.Models.Entities;
using MongoDB.Bson;

namespace ConstructorAdminAPI.Application.Services
{
    public class BuildingService
    {
        IBuildingRepository _buildingRepository;
        IMapper _mapper;

        public BuildingService(IBuildingRepository buildingRepository, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task<Result.Result> InsertBuilding(CreateBuildingDto buildingDto, CancellationToken cancellationToken)
        {
            var building = _mapper.Map<Building>(buildingDto);
            building.Id = ObjectId.GenerateNewId().ToString();

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

        public async Task<Result.Result<IReadOnlyList<Building>>> GetAllBuildings(CancellationToken cancellationToken)
        {
            var buildings = await _buildingRepository.ListAsync(cancellationToken);
            return Result.Result<IReadOnlyList<Building>>.Success(buildings);
        }
    }
}
