using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class UserUpdatePermissions
    {
        public int Id { get; set; }

        public List<UserGroupUpdatePermission> Groups { get; set; }
        public class UserGroupUpdatePermission
        {
            public int GroupId { get; set; }
            public RightsUpdatePermission Rights { get; set; }
        }
        public class RightsUpdatePermission
        {
            public string RoleName { get; set; }
            // public GroupRightsPermissions Rights { get; set; }

            public bool AllowIVR { get; set; }//operations
            public bool PerformOperations { get; set; }//operations
            public bool AllowParking { get; set; }//operations
            public bool CanIntercom { get; set; }//operations
            public bool CanSeeGroupCalls { get; set; }//presence
            public bool CanSeeGroupMembers { get; set; }//presence
            public bool AllowToChangePresence { get; set; }
            public bool AllowToManageCompanyBook { get; set; }
            public bool AssignClearOperations { get; set; }
             public bool CanSeeGroupRecordings { get; set; }
             public bool Invalid { get; set; }
            public bool ShowMyCalls { get; set; }
            public bool CanBargeIn { get; set; }//Barge-in, Listen & Whisper
            public bool ShowMyPresence { get; set; }
             public bool ShowMyPresenceOutside { get; set; }

        }

    }
}
