using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Exceptions
{
    public class OrderNotFoundException : ApplicationException
    {
        public OrderNotFoundException()
        {
            
        }

        public OrderNotFoundException(string msg): 
            base(msg)
        {
            
        }

        public OrderNotFoundException(string msg, Exception innerExcption) :
            base(msg, innerExcption)
        {

        }
    }
}
