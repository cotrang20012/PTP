using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Exceptions
{
    public class PlaceNotFoundException : Exception
    {
        public PlaceNotFoundException(string message) : base(message)
        {
        }

        public PlaceNotFoundException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
