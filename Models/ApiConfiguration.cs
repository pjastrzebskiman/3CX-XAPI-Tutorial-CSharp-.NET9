using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class ApiConfiguration
    {
        public string? BasePath { get; set; }
        public Func<Task<string>> AccessToken { get; set; }
    }
}
