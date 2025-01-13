using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class Queues
    {
        public int Id { get; set; }
        public  string Name  { get; set; }
    }

    public class QueuesResponse
    {
        public List<Queues> Value { get; set; }
    }
}
