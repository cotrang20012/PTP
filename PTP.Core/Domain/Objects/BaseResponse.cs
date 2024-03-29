﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Domain.Objects
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string ErrorMessage { get; set; } = null!;
        public int StatusCode { get; set; }
        public Object? Data { get; set; } 
    }
}
