using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Constructor_API.Application.Services
{
    public class UserService
    {
        readonly IUserRepository _userRepository;
        readonly IProjectUserRepository _projectUserRepository;
        readonly IProjectRepository _projectRepository;
        readonly IConfiguration _configuration;

        public UserService(IProjectRepository projectRepository, IUserRepository userRepository, IConfiguration configuration,
            IProjectUserRepository projectuserRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _projectUserRepository = projectuserRepository;
        }

        public async Task<string> GenerateToken(User user, CancellationToken cancellationToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (user == null || user.Id == null) throw new NotFoundException("User is not found");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["ExpireHours"])),
                signingCredentials: creds));
        }

        public async Task<string> RegisterUser(CreateUserDto userDto, CancellationToken cancellationToken)
        {
            if ((await _userRepository.CountAsync(u => u.Email == userDto.Email, cancellationToken)) != 0)
                throw new AlreadyExistsException($"User with email {userDto.Email} already exists");
            if ((await _userRepository.CountAsync(u => u.Nickname == userDto.Nickname, cancellationToken)) != 0)
                throw new AlreadyExistsException($"User with nickname {userDto.Nickname} already exists");

            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Nickname = userDto.Nickname,
                Email = userDto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FeatureIds = [],
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            };

            var token = await GenerateToken(user, cancellationToken);
            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChanges();
            return token;
        }

        public async Task<string> Login(LoginUserDto userDto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == userDto.Email, cancellationToken)
                ?? throw new NotFoundException($"User with email {userDto.Email} is not found");
            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                //NotAuthorizedException
                throw new Exception($"Wrong password");

            return await GenerateToken(user, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await _userRepository.ListAsync(cancellationToken);
        }

        public async Task<User> GetUserById(string id, CancellationToken cancellationToken)
        {
            return await _userRepository.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
                ?? throw new NotFoundException("User not found");
        }

        public async Task<IReadOnlyList<GetProjectDto>> GetProjectsByUser(string id, CancellationToken cancellationToken)
        {
            if (await _userRepository.CountAsync(u => u.Id == id, cancellationToken) == 0)
                throw new NotFoundException("User not found");

            var projectIds = (await _projectUserRepository.ListAsync(pu => pu.UserId == id, cancellationToken)).Select(x => x.ProjectId);
            return await _projectRepository.SimpleGetProjectDtoListAsync(p => projectIds.Any(id => id == p.Id), cancellationToken);
        }

        public async Task DeleteUser(string id, CancellationToken cancellationToken)
        {
            await _userRepository.RemoveAsync(u => u.Id == id, cancellationToken);
            await _userRepository.SaveChanges();
        }

        public async Task UpdateUser(string id, UpdateUserDto userDto, CancellationToken cancellationToken)
        {
            var prevUser = await _userRepository.FirstAsync(u => u.Id == id, cancellationToken)
                ?? throw new NotFoundException("User not found");

            prevUser.Nickname = userDto.Nickname ?? prevUser.Nickname;
            prevUser.Email = userDto.Email ?? prevUser.Email;
            if (userDto.Password != null)
                prevUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            prevUser.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(u => u.Id == id, prevUser, cancellationToken);
            await _userRepository.SaveChanges();
        }

        //public async Task<Project> GetUserProjectByName(string id, string name,
        //    CancellationToken cancellationToken)
        //{
        //    var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        //    if (user == null) throw new NotFoundException("User not found");

        //    if (user.ProjectUserIds != null)
        //    {
        //        var project = await _projectRepository.FirstOrDefaultAsync(p => user.ProjectUserIds.Contains(p.Id) && 
        //            p.Name == name, cancellationToken);
        //        if (project == null) throw new NotFoundException("Project not found");
        //        return project;
        //    }
        //    else throw new NotFoundException("Project not found");
        //}
    }
}
