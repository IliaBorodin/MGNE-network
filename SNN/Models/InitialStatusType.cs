using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Models
{
    public class InitialStatusType
    {
        public string Name { get; set; }
        public int Type { get; set; } // 1 for bidirectional, 2 for first->second, 3 for second->first
    }
}
