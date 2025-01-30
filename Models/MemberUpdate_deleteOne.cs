using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class MemberUpdate_deleteOne
    {
        [JsonPropertyName("Number")]
        public string Number { get; set; }
        [JsonPropertyName("Id")]
        public int Id { get; set; }
    }
    public class GroupMembersUpdate_deleteOne
    {
        [JsonPropertyName("value")]
        public List<MemberUpdate_deleteOne> Value { get; set; } = new List<MemberUpdate_deleteOne>();
        // public List<MemberUpdate_deleteOne> Members { get; set; } = new List<MemberUpdate_deleteOne>();
    }

    public class MemberUpdate_deleteOne_add
    {
        [JsonPropertyName("Members")]
        public List<MemberUpdate_deleteOne> Members { get; set; } = new List<MemberUpdate_deleteOne>();

    }
}
