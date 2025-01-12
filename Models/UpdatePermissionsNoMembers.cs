using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class UpdatePermissionsNoMembers
    {
        public int Id { get; set; }

        public List<UserGroupUpdatePermissionNoMembers> Groups { get; set; }
        public class UserGroupUpdatePermissionNoMembers
        {
            public int GroupId { get; set; }
            public RightsUpdatePermissionNoMembers Rights { get; set; }
        }
        public class RightsUpdatePermissionNoMembers
        {
            public string RoleName { get; set; }

            public bool AllowIVR { get; set; }//operations
            public bool AllowParking { get; set; }//operations
            public bool CanIntercom { get; set; }//operations
            public bool CanSeeGroupCalls { get; set; }//presence
            public bool CanSeeGroupMembers { get; set; }//presence
            public bool PerformOperations { get; set; }//operations

        }

     
    }
}
