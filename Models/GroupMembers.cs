using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class GroupMembers
    {
        [JsonPropertyName("value")]
        public List<GroupMember> Value { get; set; }

    }
}
