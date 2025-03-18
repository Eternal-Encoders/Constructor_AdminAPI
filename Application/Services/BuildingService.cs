using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using MongoDB.Bson;

namespace Constructor_API.Application.Services
{
    public class BuildingService
    {
        IBuildingRepository _buildingRepository;
        IProjectRepository _projectRepository;
        IFloorRepository _floorRepository;
        IGraphPointRepository _graphPointRepository;
        IMapper _mapper;

        public BuildingService(IBuildingRepository buildingRepository, IMapper mapper,
            IProjectRepository projectRepository, IFloorRepository floorRepository,
            IGraphPointRepository graphPointRepository)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
            _projectRepository = projectRepository;
            _floorRepository = floorRepository;
            _graphPointRepository = graphPointRepository;
        }

        public async Task InsertBuilding(CreateBuildingDto buildingDto, CancellationToken cancellationToken)
        {
            var building = _mapper.Map<Building>(buildingDto);
            building.Id = ObjectId.GenerateNewId().ToString();

            var project = await _projectRepository.FirstAsync(g => g.Id == buildingDto.ProjectId, cancellationToken);
            if (project == null) throw new NotFoundException("Project not found");
            else 
            {
                project.BuildingIds.Append(building.Id);
                await _projectRepository.UpdateAsync(g => g.Id == buildingDto.ProjectId, project, cancellationToken);
            }

            await _buildingRepository.AddAsync(building, cancellationToken);
            await _buildingRepository.SaveChanges();
        }

        public async Task<Building> GetBuildingById(string id, CancellationToken cancellationToken)
        {
            var building = await _buildingRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if (building == null) throw new NotFoundException("Building not found");

            return building;
        }

        public async Task<IReadOnlyList<Floor>> GetFloorsByBuilding(string buildingId, CancellationToken cancellationToken)
        {
            var res = await _floorRepository.ListAsync(f => f.BuildingId == buildingId, cancellationToken);
            return res;
        }

        public async Task<IReadOnlyList<GetFloorDto>> GetFloorsByBuildingWithGraphPoints(string buildingId,
            CancellationToken cancellationToken)
        {
            var floors = await _floorRepository.ListAsync(f => f.BuildingId == buildingId, cancellationToken);
            List<GetFloorDto> res = [];

            for(int i = 0; i < floors.Count; i++)
            {
                res.Add(_mapper.Map<GetFloorDto>(floors[i]));

                res[i].GraphPoints = (await _graphPointRepository.ListAsync(g => g.FloorId == floors[i].Id, cancellationToken))
                    .ToArray();
            }

            return res;
        }

        public async Task<Floor> GetFloorInBuildingByNumber(string buildingId, int number, CancellationToken cancellationToken)
        {
            var res = await _floorRepository.FirstOrDefaultAsync(f => f.BuildingId == buildingId &&
                f.FloorNumber == number, cancellationToken);
            if (res == null) throw new NotFoundException("Floor not found");
            return res;
        }

        public async Task<GetFloorDto> GetFloorInBuildingByNumberWithGraphPoints(string buildingId,
            int number, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(f => f.BuildingId == buildingId &&
                f.FloorNumber == number, cancellationToken);

            var res = _mapper.Map<GetFloorDto>(floor);
            res.GraphPoints = (await _graphPointRepository.ListAsync(g => g.FloorId == floor.Id, cancellationToken))
                    .ToArray();

            return res;
        }

        public async Task<IReadOnlyList<Building>> GetAllBuildings(CancellationToken cancellationToken)
        {
            var buildings = await _buildingRepository.ListAsync(cancellationToken);
            return buildings;
        }
    }
}
