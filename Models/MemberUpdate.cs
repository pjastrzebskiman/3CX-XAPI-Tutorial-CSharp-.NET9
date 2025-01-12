using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class MemberUpdate
    {
        [JsonPropertyName("Number")]
        public string Number { get; set; }
    }
    public class GroupMembersUpdate
    {
        [JsonPropertyName("Members")]
        public List<MemberUpdate> Members { get; set; } = new List<MemberUpdate>();
    }
}
