using Microsoft.AspNetCore.Authorization;

namespace Constructor_API.Application.Authorization.Requirements
{
    public class TypeRequirement : IAuthorizationRequirement
    {
        public string ResourceType { get; }
        //public string? ResourceId { get; }

        public TypeRequirement(string ResourceType)
        {
            this.ResourceType = ResourceType;
            //this.ResourceId = ResourceId;
        }
    }
}
