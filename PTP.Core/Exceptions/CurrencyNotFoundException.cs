using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Exceptions
{
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException(string message) : base(message)
        {
        }

        public CurrencyNotFoundException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
