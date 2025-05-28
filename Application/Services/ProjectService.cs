using AutoMapper;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System;

namespace Constructor_API.Application.Services
{
    public class ProjectService
    {
        IProjectRepository _projectRepository;
        IBuildingRepository _buildingRepository;
        IUserRepository _userRepository;
        IMapper _mapper;
        IProjectUserRepository _projectUserRepository;
        ImageService _imageService;

        public ProjectService(
            IProjectRepository projectRepository, 
            IBuildingRepository buildingRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IProjectUserRepository projectUserRepository,
            ImageService imageService)
        {
            _projectRepository = projectRepository;
            _buildingRepository = buildingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _projectUserRepository = projectUserRepository;
            _imageService = imageService;
        }

        public async Task<GetProjectDto> InsertProject(
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
            project.Id = ObjectId.GenerateNewId().ToString();

            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
                ?? throw new NotFoundException("User is not found");

            var projUser = new ProjectUser
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ProjectId = project.Id,
                UserId = userId,
                ProjectRole = "admin",
                AddedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            user.SelectedProject = project.Id;
            await _userRepository.UpdateAsync(u => u.Id == userId, user, cancellationToken);
            await _projectUserRepository.AddAsync(projUser, cancellationToken);
            await _projectRepository.AddAsync(project, cancellationToken);
            await _projectRepository.SaveChanges();

            return _mapper.Map<GetProjectDto>(project);
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
            await _projectRepository.SaveChanges();

            return project;
        }

        //public async Task<Tuple<Image?, MultipartContent>> GetProjectByIdMultipart(
        //    string id, string userId, CancellationToken cancellationToken)
        //{
        //    var project = await _projectRepository.FirstGetProjectDtoOrDefaultAsync(p => p.Id == id, cancellationToken)
        //        ?? throw new NotFoundException("Project is not found");
        //    var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
        //        ?? throw new NotFoundException("User is not found");

        //    user.SelectedProject = id;

        //    MultipartContent multipartContent = new MultipartContent("mixed");

        //    var jsonContent = new StringContent(JsonSerializer.Serialize(project), Encoding.UTF8, "application/json");
        //    multipartContent.Add(jsonContent);

        //    if (project.Icon != null)
        //    {
        //        if (project.Icon != "")
        //        {
        //            var tuple = await _imageService.GetIconById(project.Icon, CancellationToken.None);

        //            var fileContent = new ByteArrayContent(tuple.Item2.ToArray());
        //            fileContent.Headers.ContentType = new MediaTypeHeaderValue(tuple.Item1.MimeType);
        //            multipartContent.Add(fileContent);
        //            return new Tuple<Image?, MultipartContent>(tuple.Item1, multipartContent);
        //        }
        //    }
        //    else
        //    {
        //        //Если не было значения
        //        var updatedProject = await _projectRepository.FirstAsync(p => p.Id == id, cancellationToken)
        //            ?? throw new NotFoundException("Project is not found");
        //        updatedProject.Icon = "";
        //        await _projectRepository.UpdateAsync(p => p.Id == id, updatedProject, cancellationToken);
        //    }

        //    await _userRepository.UpdateAsync(u => u.Id == userId, user, cancellationToken);
        //    await _projectRepository.SaveChanges();

        //    return new Tuple<Image?, MultipartContent>(null, multipartContent);
        //}

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
            //if (await _buildingRepository.CountAsync(b => b.ProjectId == id, cancellationToken) == 0)
            //    throw new NotFoundException($"Buildings are not found");
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
