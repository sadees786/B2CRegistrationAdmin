using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RegistrationAdmin.Authorization.Handlers
{
    public class GroupCheckHandler : AuthorizationHandler<GroupsCheckRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupsCheckRequirement requirement)
        {
            if (IsInGroups(context.User, requirement))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        private bool IsInGroups(ClaimsPrincipal user, GroupsCheckRequirement glist)
        {
            foreach (var group in glist.Grouplist)
            {
                var groupcheck = (user.Claims.Any(claim => claim.Type == "groups" &&
                               claim.Value.Equals(group.GroupId)));
                if (groupcheck) { return true; }
            }
            return false;
        }

    }
}
