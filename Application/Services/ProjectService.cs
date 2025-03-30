using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using Constructor_API.Models.InnerObjects;
using MongoDB.Bson;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Constructor_API.Application.Services
{
    public class ProjectService
    {
        IProjectRepository _projectRepository;
        IBuildingRepository _buildingRepository;
        IFloorRepository _floorRepository;
        IGraphPointRepository _graphPointRepository;
        IFloorConnectionRepository _floorConnectionRepository;
        IUserRepository _userRepository;

        public ProjectService(IProjectRepository projectRepository, IBuildingRepository buildingRepository,
            IUserRepository userRepository, IFloorRepository floorRepository, 
            IGraphPointRepository floorGraphPointRepository, IFloorConnectionRepository floorConnectionRepository)
        {
            _projectRepository = projectRepository;
            _buildingRepository = buildingRepository;
            _userRepository = userRepository;
            _floorRepository = floorRepository;
            _graphPointRepository = floorGraphPointRepository;
            _floorConnectionRepository = floorConnectionRepository;
        }

        public async Task InsertProject(
            CreateProjectDto projectDto, string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
                ?? throw new NotFoundException("User is not found");

            Project project = new Project();
            project.Name = projectDto.Name;
            project.Description = projectDto.Description;
            project.ProjectUsers = [];
            project.CustomGraphPointTypes = [];
            //[ new ProjectUser { } ];
            //project.ImageId = projectDto.Image;
            project.BuildingIds = [];
            project.Id = ObjectId.GenerateNewId().ToString();

            user.ProjectIds?.Append(project.Id);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(u => u.Id == user.Id, user, cancellationToken);
            await _projectRepository.AddAsync(project, cancellationToken);
            await _projectRepository.SaveChanges();
        }

        //public async Task<Project> GetProjectByName(
        //    string name, CancellationToken cancellationToken)
        //{
        //    var res = await _projectRepository.FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
        //    if (res == null) throw new NotFoundException("Navigation group not found");

        //    return res;
        //}

        public async Task<Project> GetProjectById(
            string id, CancellationToken cancellationToken)
        {
            var res = await _projectRepository.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (res == null) throw new NotFoundException("Navigation group not found");

            return res;
        }

        public async Task<IReadOnlyList<Project>> GetAllProjects(
            CancellationToken cancellationToken)
        {
            var res = await _projectRepository.ListAsync(cancellationToken);
            return res;
        }

        public async Task<IReadOnlyList<Building>> GetBuildingsByProject(string projectId,
            CancellationToken cancellationToken)
        {
            var res = await _buildingRepository.ListAsync(b => b.ProjectId == projectId, cancellationToken);
            return res;
        }

        public async Task<Building> GetBuildingInProjectByName(string projectId, string name,
            CancellationToken cancellationToken)
        {
            var building = await _buildingRepository.FirstOrDefaultAsync(b => b.ProjectId == projectId && b.Name == name,
                cancellationToken);
            if (building == null) throw new NotFoundException("Building not found");

            return building;
        }

        public async Task DeleteProject(string id, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
                ?? throw new NotFoundException($"Project is not found");
            var buildings = await _buildingRepository.ListAsync(b => b.ProjectId == id, cancellationToken)
                ?? throw new NotFoundException($"Buildings are not found");
            //var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            //    ?? throw new NotFoundException("User is not found");
            //user.ProjectIds = user.ProjectIds.Where(p => p != id).ToArray();
            //user.UpdatedAt = DateTime.UtcNow;

            await _floorConnectionRepository.RemoveRangeAsync(c => buildings.Any(b => b.Id == c.BuildingId), cancellationToken);

            foreach (var building in buildings)
            {
                if (building.FloorIds != null)
                {
                    foreach (var floorId in building.FloorIds)
                    {
                        var floor = await _floorRepository.FirstOrDefaultAsync(b => b.Id == floorId, cancellationToken)
                            ?? throw new NotFoundException("Floor is not found");

                        await _graphPointRepository.RemoveRangeAsync(g => floor.GraphPoints.Contains(g.Id), cancellationToken);
                    }
                    await _floorRepository.RemoveRangeAsync(f => f.BuildingId == building.Id, cancellationToken);
                }
            }

            await _buildingRepository.RemoveRangeAsync(b => b.ProjectId == id, cancellationToken);
            await _projectRepository.RemoveAsync(p => p.Id == id, cancellationToken);
            await _buildingRepository.SaveChanges();
        }
    }
}
