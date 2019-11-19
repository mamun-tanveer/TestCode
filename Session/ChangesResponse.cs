using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class ChangesResponse
    {
        public bool HasChanged { get; set; }
        public DateTime Since { get; set; }
    }
}
