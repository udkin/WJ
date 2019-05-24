using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WJ.API.Models
{
    public class ResponseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}