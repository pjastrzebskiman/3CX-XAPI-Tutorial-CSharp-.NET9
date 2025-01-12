using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class GroupMembersDeleteAll
    {
        [JsonPropertyName("Members")]
        public List<object> Members { get; set; } = new List<object>();
    }
}
