using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseCompare
{
    class RegexCache : IRegexCache
    {

        public RegexCache(string filePath)
        {

        }    

        public IEnumerable<string> GetRegexes(string url)
        {
            throw new NotImplementedException();
        }
    }
}
