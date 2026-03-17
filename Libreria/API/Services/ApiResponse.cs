using System;
using System.Collections.Generic;
using System.Text;

namespace HTTPRequestsLibrary.Services
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int Error { get; set; }
        public string ErrorString { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
    }
}
