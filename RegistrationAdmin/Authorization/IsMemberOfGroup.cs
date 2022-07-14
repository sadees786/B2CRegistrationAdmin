using Microsoft.AspNetCore.Authorization;


namespace RegistrationAdmin.Authorization
{
    public class IsMemberOfGroup : IAuthorizationRequirement
    {
        public readonly string GroupId;
        public readonly string GroupName;

        public IsMemberOfGroup(string groupName, string groupId)
        {
            GroupName = groupName;
            GroupId = groupId;
        }
    }
}

