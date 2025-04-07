using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApi.Application.Exceptions
{
    public class InvalidSaleException : Exception
    {
        public InvalidSaleException(string message) : base(message)
        {
        }
    }
}

