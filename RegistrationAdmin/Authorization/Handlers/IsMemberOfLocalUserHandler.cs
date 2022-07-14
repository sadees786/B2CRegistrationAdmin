using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace RegistrationAdmin.Authorization.Handlers
{
    public class IsMemberOfLocalUserHandler : AuthorizationHandler<IsMemberOfGroup>
    {
        //Bye pass the Authorization for local environment
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsMemberOfGroup memberOfGroup)
        {
                context.Succeed(memberOfGroup);
            return Task.CompletedTask;
        }
    }
}


