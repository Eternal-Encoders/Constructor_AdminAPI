using Constructor_API.Core.Repositories;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using System.Threading;
using System.Xml.Linq;

namespace Constructor_API.Application.Services
{
    public class NavigationGroupService
    {
        INavigationGroupRepository _navigationGroupRepository;

        public NavigationGroupService(INavigationGroupRepository navigationGroupRepository)
        {
            _navigationGroupRepository = navigationGroupRepository;
        }

        public async Task<Result.Result> InsertNavigationGroup(
            CreateNavigationGroupDto navigationGroupDto, CancellationToken cancellationToken)
        {
            NavigationGroup navGroup = new NavigationGroup();
            navGroup.Name = navigationGroupDto.Name;
            navGroup.BuildingIds = [];
            navGroup.Id = ObjectId.GenerateNewId().ToString();

            await _navigationGroupRepository.AddAsync(navGroup, cancellationToken);
            await _navigationGroupRepository.SaveChanges();

            return Result.Result.Success();
        }

        public async Task<Result.Result<NavigationGroup>> GetNavigationGroupByName(
            string name, CancellationToken cancellationToken)
        {
            var res = await _navigationGroupRepository.FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
            if (res == null) return Result.Result<NavigationGroup>.Error(new Result.Error(
                "Navigation group not found", 404));

            return Result.Result<NavigationGroup>.Success(res);
        }

        public async Task<Result.Result<NavigationGroup>> GetNavigationGroupById(
            string id, CancellationToken cancellationToken)
        {
            var res = await _navigationGroupRepository.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (res == null) return Result.Result<NavigationGroup>.Error(new Result.Error(
                "Navigation group not found", 404));

            return Result.Result<NavigationGroup>.Success(res);
        }

        public async Task<Result.Result<IReadOnlyList<NavigationGroup>>> GetAllNavigationGroups(
            CancellationToken cancellationToken)
        {
            var res = await _navigationGroupRepository.ListAsync(cancellationToken);
            return Result.Result<IReadOnlyList<NavigationGroup>>.Success(res);
        }
    }
}
