using Microsoft.AspNetCore.Authorization;
using RegistrationAdmin.Models.B2C;
using System.Collections.Generic;

namespace RegistrationAdmin.Authorization
{
   public class GroupsCheckRequirement: IAuthorizationRequirement
    {
        public List<AzureGroupConfig> Grouplist { get; }
        public GroupsCheckRequirement(List<AzureGroupConfig> groupOptions)
         {
            Grouplist = groupOptions;
         }
     }
}
