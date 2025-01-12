using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CX_API_20
{
    public  interface IUserUpdatePermissionNoMember
    {
        string RoleName { get; set; }
        bool AllowIVR { get; set; }
        bool AllowParking { get; set; }
        bool CanIntercom { get; set; }
        bool CanSeeGroupCalls { get; set; }
        bool CanSeeGroupMembers { get; set; }
        bool PerformOperations { get; set; }
    }

    public interface IUserUpdatePermission
    {
        int GroupId { get; set; }
        IUserUpdatePermissionNoMember Rights { get; set; }
    }
}
