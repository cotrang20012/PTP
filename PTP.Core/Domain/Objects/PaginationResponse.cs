using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Domain.Objects
{
    public class PaginationResponse : BaseResponse
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
    }
}
