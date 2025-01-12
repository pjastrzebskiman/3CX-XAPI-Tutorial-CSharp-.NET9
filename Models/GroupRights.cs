using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class GroupRights
    {
            public bool AllowIVR { get; set; }
            public bool AllowParking { get; set; }
            public bool AllowToChangePresence { get; set; }
            public bool AllowToManageCompanyBook { get; set; }
            public bool AssignClearOperations { get; set; }
            public bool CanBargeIn { get; set; }
            public bool CanIntercom { get; set; }
            public bool CanSeeGroupCalls { get; set; }
            public bool CanSeeGroupMembers { get; set; }
            public bool CanSeeGroupRecordings { get; set; }
            public bool Invalid { get; set; }
            public bool PerformOperations { get; set; }
            public string RoleName { get; set; }
            public bool ShowMyCalls { get; set; }
            public bool ShowMyPresence { get; set; }
            public bool ShowMyPresenceOutside { get; set; }
    }
}
