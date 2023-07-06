using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Exceptions
{
    public class CountryNotFoundException : Exception
    {
        public CountryNotFoundException(string message) : base(message)
        {
        }

        public CountryNotFoundException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
