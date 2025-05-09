using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using System.Linq;

namespace Constructor_API.Application.Services
{
    public class ProjectService
    {
        IProjectRepository _projectRepository;
        IBuildingRepository _buildingRepository;
        IUserRepository _userRepository;
        IMapper _mapper;
        IProjectUserRepository _projectUserRepository;

        public ProjectService(
            IProjectRepository projectRepository, 
            IBuildingRepository buildingRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IProjectUserRepository projectUserRepository)
        {
            _projectRepository = projectRepository;
            _buildingRepository = buildingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _projectUserRepository = projectUserRepository;
        }

        public async Task<Project> InsertProject(
            CreateProjectDto projectDto, 
            string userId, 
            CancellationToken cancellationToken)
        {
            if (await _userRepository.CountAsync(u => u.Id == userId, cancellationToken) == 0)
                throw new NotFoundException("User is not found");

            Project project = _mapper.Map<Project>(projectDto);
            project.CustomGraphPointTypes = [];
            project.BuildingIds = [];
            if (await _projectRepository.CountAsync(p => p.Url == projectDto.Url, cancellationToken) != 0)
                throw new AlreadyExistsException($"Project with url {projectDto.Url} already exists");
            //project.ImageId = projectDto.Image;
            project.Id = ObjectId.GenerateNewId().ToString();

            var projUser = new ProjectUser
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ProjectId = project.Id,
                UserId = userId,
                ProjectRole = "admin",
                AddedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _projectUserRepository.AddAsync(projUser, cancellationToken);
            await _projectRepository.AddAsync(project, cancellationToken);
            await _projectRepository.SaveChanges();

            return project;
        }

        public async Task<GetProjectDto> GetProjectById(
            string id, string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
                ?? throw new NotFoundException("User is not found");
            var project = await _projectRepository.FirstGetProjectDtoOrDefaultAsync(p => p.Id == id, cancellationToken)
                ?? throw new NotFoundException("Project is not found");

            user.SelectedProject = id;
            await _userRepository.UpdateAsync(u => u.Id == userId, user, cancellationToken);

            return project;
        }

        public async Task<IReadOnlyList<Project>> GetAllProjects(
            CancellationToken cancellationToken)
        {
            return await _projectRepository.ListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<GetBuildingDto>> GetBuildingsByProject(string projectId,
            CancellationToken cancellationToken)
        {
            return await _buildingRepository.SimpleGetBuildingDtoListAsync(b => b.ProjectId == projectId, cancellationToken);
        }

        public async Task DeleteProject(string id, string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
               ?? throw new NotFoundException("User is not found");
            if (user.SelectedProject == id)
            {
                user.SelectedProject = "";
                await _userRepository.UpdateAsync(u => u.Id == userId, user, cancellationToken);
            }
            if (await _projectRepository.CountAsync(p => p.Id == id, cancellationToken) == 0)
                throw new NotFoundException($"Project is not found");
            if (await _buildingRepository.CountAsync(b => b.ProjectId == id, cancellationToken) == 0)
                throw new NotFoundException($"Buildings are not found");
            await _projectRepository.RemoveAsync(p => p.Id == id, cancellationToken);
            await _projectRepository.SaveChanges();
        }

        public async Task UpdateProject(string id, UpdateProjectDto projectDto, CancellationToken cancellationToken)
        {
            var prevProject = await _projectRepository.FirstOrDefaultAsync(p => 
                p.Id == id, cancellationToken) ?? throw new NotFoundException($"Project {id} is not found");

            prevProject.Name = projectDto.Name ?? prevProject.Name;
            prevProject.Url = projectDto.Url ?? prevProject.Url;
            prevProject.Description = projectDto.Description ?? prevProject.Description;
            //prevProject.ImageId = projectDto.ImageId ?? prevProject.ImageId;
            prevProject.CustomGraphPointTypes = projectDto.CustomGraphPointTypes ?? prevProject.CustomGraphPointTypes;
            prevProject.UpdatedAt = DateTime.UtcNow;

            await _projectRepository.UpdateAsync(p => p.Id == id, prevProject, cancellationToken);
            await _projectRepository.SaveChanges();
        }
    }
}
