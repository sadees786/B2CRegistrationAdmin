using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAdmin.Authorization.Handlers
{
    public class IsMemberOfGroupHandler : AuthorizationHandler<IsMemberOfGroup>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, IsMemberOfGroup memberOfGroup)
        {
            var groupClaim = context.User.Claims
                 .FirstOrDefault(claim => claim.Type == "groups" &&
                     claim.Value.Equals(memberOfGroup.GroupId, StringComparison.InvariantCultureIgnoreCase));
            if (groupClaim != null)
                context.Succeed(memberOfGroup);
            return Task.CompletedTask;
        }
    }
}


