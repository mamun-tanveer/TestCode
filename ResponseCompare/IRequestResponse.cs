using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseCompare
{
    public interface IRequestResponse
    {
        void MakeRequest();
        string Serialize();
        string Url { get; }
        string ID { get; }
        string ResponseText { get; }

        IEnumerable<string> RegExs { get; }
        int ResponseCode { get; }       
    }
}
