using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Exceptions
{
    public class BadUserInputException : Exception
    {
        public BadUserInputException(string message) : base(message)
        {
        }

        public BadUserInputException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
