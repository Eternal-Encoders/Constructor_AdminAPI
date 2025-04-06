using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
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
        IFloorConnectionRepository _floorConnectionRepository;
        IMapper _mapper;

        public BuildingService(IBuildingRepository buildingRepository, IMapper mapper,
            IProjectRepository projectRepository, IFloorRepository floorRepository,
            IGraphPointRepository graphPointRepository, IFloorConnectionRepository floorConnectionRepository)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
            _projectRepository = projectRepository;
            _floorRepository = floorRepository;
            _graphPointRepository = graphPointRepository;
            _floorConnectionRepository = floorConnectionRepository;
        }

        public async Task InsertBuilding(CreateBuildingDto buildingDto, CancellationToken cancellationToken)
        {
            var building = _mapper.Map<Building>(buildingDto);
            building.Id = ObjectId.GenerateNewId().ToString();
            building.FloorIds = [];

            var project = await _projectRepository.FirstAsync(g => g.Id == buildingDto.ProjectId, cancellationToken);
            if (project == null) throw new NotFoundException("Project is not found");
            else 
            {
                project.UpdatedAt = DateTime.UtcNow;
                if (project.BuildingIds != null)
                    project.BuildingIds = [..project.BuildingIds.Append(building.Id)];
                else
                    project.BuildingIds = [building.Id];
                await _projectRepository.UpdateAsync(g => g.Id == buildingDto.ProjectId, project, cancellationToken);
            }

            await _buildingRepository.AddAsync(building, cancellationToken);
            await _buildingRepository.SaveChanges();
        }

        public async Task<Building> GetBuildingById(string id, CancellationToken cancellationToken)
        {
            var building = await _buildingRepository.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if (building == null) throw new NotFoundException("Building is not found");

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

        public async Task<IReadOnlyList<GraphPoint>> GetPointsByBuildingAndType(string buildingId, string type,
            CancellationToken cancellationToken)
        {
            var floors = await _floorRepository.ListAsync(f => f.BuildingId == buildingId, cancellationToken);
            List<GraphPoint> res = [];

            for (int i = 0; i < floors.Count; i++)
            {
                res.AddRange(await _graphPointRepository.ListAsync(g => g.FloorId == floors[i].Id &&
                    g.Types.Contains(type), cancellationToken));
            }

            return res;
        }

        public async Task<Floor> GetFloorInBuildingByNumber(string buildingId, int number, CancellationToken cancellationToken)
        {
            var res = await _floorRepository.FirstOrDefaultAsync(f => f.BuildingId == buildingId &&
                f.FloorNumber == number, cancellationToken);
            if (res == null) throw new NotFoundException("Floor is not found");
            return res;
        }

        public async Task<GetFloorDto> GetFloorInBuildingByNumberWithGraphPoints(string buildingId,
            int number, CancellationToken cancellationToken)
        {
            var floor = await _floorRepository.FirstOrDefaultAsync(f => f.BuildingId == buildingId &&
                f.FloorNumber == number, cancellationToken);
            if (floor == null) throw new NotFoundException($"Floor is not found");

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

        public async Task DeleteBuilding(string buildingId, CancellationToken cancellationToken)
        {
            var building = await _buildingRepository.FirstOrDefaultAsync(b => b.Id == buildingId, cancellationToken)
                ?? throw new NotFoundException($"Building is not found");
            var project = await _projectRepository.FirstOrDefaultAsync(p => p.Id == building.ProjectId, cancellationToken)
                ?? throw new NotFoundException($"Project is not found");

            project.BuildingIds = project.BuildingIds.Where(b => b != buildingId).ToArray();
            project.UpdatedAt = DateTime.UtcNow;
            await _projectRepository.UpdateAsync(p => p.Id == project.Id, project, cancellationToken);

            await _floorConnectionRepository.RemoveRangeAsync(c => c.BuildingId == buildingId, cancellationToken);

            if (building.FloorIds != null)
            {
                foreach (var floorId in building.FloorIds)
                {
                    var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == floorId, cancellationToken)
                        ?? throw new NotFoundException("Floor is not found");

                    await _graphPointRepository.RemoveRangeAsync(g => floor.GraphPoints.Contains(g.Id), cancellationToken);
                }
            }
            await _floorRepository.RemoveRangeAsync(f => f.BuildingId == buildingId, cancellationToken);
            await _buildingRepository.RemoveAsync(b => b.Id == buildingId, cancellationToken);
            await _buildingRepository.SaveChanges();
        }
    }
}
