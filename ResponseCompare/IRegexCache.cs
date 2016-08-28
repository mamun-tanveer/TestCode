using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseCompare
{
    interface IRegexCache
    {
        IEnumerable<string> GetRegexes(string url);
    }
}
