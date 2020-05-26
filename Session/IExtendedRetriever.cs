using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    interface IExtendedRetriever
    {
        ISessionObject Retrieve(string idText);
    }
}
