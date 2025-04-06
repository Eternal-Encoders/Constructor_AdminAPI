using Constructor_API.Application.Authorization.Requirements;
using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using System.Linq;
using System.Security.Claims;

namespace Constructor_API.Application.Authorization.Handlers
{
    public class UserAuthorizationHandler : AuthorizationHandler<TypeRequirement, string>
    {
        private readonly IProjectUserRepository _projectUserRepository;

        public UserAuthorizationHandler(IProjectUserRepository projectUserRepository)
        {
            _projectUserRepository = projectUserRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
           TypeRequirement requirement, string id)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || userIdClaim.Value == null)
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            if (requirement == null || id == null || !ObjectId.TryParse(id, out _)
                || requirement.ResourceType == null)
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            switch (requirement.ResourceType.ToLower())
            {
                case "project":
                    {
                        if ((await _projectUserRepository.GetUsersForProject(id)).Any(i =>
                            i == userIdClaim.Value))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                        }
                        else
                        {
                            context.Fail();
                            await Task.CompletedTask;
                            return;
                        }
                        break;
                    }

                case "building":
                    {
                        if ((await _projectUserRepository.GetUsersForBuilding(id)).Any(i =>
                            i == userIdClaim.Value))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                        }
                        else
                        {
                            context.Fail();
                            await Task.CompletedTask;
                            return;
                        }
                        break;
                    }

                case "floor":
                    {
                        if ((await _projectUserRepository.GetUsersForFloor(id)).Any(i =>
                            i == userIdClaim.Value))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                        }
                        else
                        {
                            context.Fail();
                            await Task.CompletedTask;
                            return;
                        }
                        break;
                    }

                case "graphpoint":
                    {
                        if ((await _projectUserRepository.GetUsersForGraphPoint(id)).Any(i =>
                            i == userIdClaim.Value))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                        }
                        else
                        {
                            context.Fail();
                            await Task.CompletedTask;
                            return;
                        }
                        break;
                    }

                case "floorconnection":
                    {
                        if ((await _projectUserRepository.GetUsersForFloorConnection(id)).Any(i =>
                            i == userIdClaim.Value))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                        }
                        else
                        {
                            context.Fail();
                            await Task.CompletedTask;
                            return;
                        }
                        break;
                    }

                case null:
                    {
                        context.Fail();
                        await Task.CompletedTask;
                        break;
                    }
            }
        }
    }
}
