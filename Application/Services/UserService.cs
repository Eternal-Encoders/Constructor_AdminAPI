using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;

namespace Constructor_API.Application.Services
{
    public class UserService
    {
        IUserRepository _userRepository;
        IProjectRepository _projectRepository;

        public UserService(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task RegisterUser(CreateUserDto userDto, CancellationToken cancellationToken)
        {

        }

        public async Task Login(LoginUserDto userDto, CancellationToken cancellationToken)
        {

        }

        public async Task<IReadOnlyList<User>> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userRepository.ListAsync(cancellationToken);
            return users;
        }

        public async Task<User> GetUserById(string id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null) throw new NotFoundException("User not found");

            return user;
        }

        public async Task<IReadOnlyList<Project>> GetProjectsByUser(string id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null) throw new NotFoundException("User not found");

            if (user.ProjectIds != null)
            {
                var projects = await _projectRepository.ListAsync(p => user.ProjectIds.Contains(p.Id), cancellationToken);
                return projects;
            }
            else return [];
        }

        public async Task<Project> GetUserProjectByName(string id, string name,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null) throw new NotFoundException("User not found");

            if (user.ProjectIds != null)
            {
                var project = await _projectRepository.FirstOrDefaultAsync(p => user.ProjectIds.Contains(p.Id) && 
                    p.Name == name, cancellationToken);
                if (project == null) throw new NotFoundException("Project not found");
                return project;
            }
            else throw new NotFoundException("Project not found");
        }
    }
}
