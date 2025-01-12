using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class UsersUpdate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public List<UserGroupUpdate> Groups { get; set; }

    }

    public class UserGroupUpdate
    {
        public int GroupId { get; set; }
        public RightsUpdate Rights { get; set; }
    }

    public class RightsUpdate
    {
        public RoleNameEnum RoleName { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleNameEnum
    {
        [JsonPropertyName("system_owners")]
        SystemOwners,

        [JsonPropertyName("system_admins")]
        SystemAdmins,

        [JsonPropertyName("group_owners")]
        GroupOwners,

        [JsonPropertyName("managers")]
        Managers,

        [JsonPropertyName("group_admins")]
        GroupAdmins,

        [JsonPropertyName("receptionists")]
        Receptionists,

        [JsonPropertyName("users")]
        Users
    }
}
