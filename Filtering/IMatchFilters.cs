using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filtering
{
    interface IMatchFilters<TMatch>
    {
        IEnumerable<TMatch> FindMatches(IEnumerable<Filter> filters);
    }
}
