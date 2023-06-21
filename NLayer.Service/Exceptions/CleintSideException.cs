using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Exceptions
{
    public class CleintSideException : Exception
    {
        public CleintSideException(string mesage):base(mesage)
        {
            
        }
    }
}
