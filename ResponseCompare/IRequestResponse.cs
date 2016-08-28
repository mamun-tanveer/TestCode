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
        string SerializeRequest();

        string ResponseText
        {
            get; 
        }

        int ResponseCode
        {
            get; 
        }       
    }
}
