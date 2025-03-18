using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using System.Threading;
using System.Xml.Linq;

namespace Constructor_API.Application.Services
{
    public class ProjectService
    {
        IProjectRepository _projectRepository;
        IBuildingRepository _buildingRepository;

        public ProjectService(IProjectRepository projectRepository, IBuildingRepository buildingRepository)
        {
            _projectRepository = projectRepository;
            _buildingRepository = buildingRepository;
        }

        public async Task InsertProject(
            CreateProjectDto projectDto, CancellationToken cancellationToken)
        {
            Project project = new Project();
            project.Name = projectDto.Name;
            project.BuildingIds = [];
            project.Id = ObjectId.GenerateNewId().ToString();

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
    }
}
