using System;
using System.Collections.Generic;
using System.Text;

namespace DWDW_API.Core.ViewModels
{
    public class ResponseViewModel
    {
        public int StatusCode { get; set; }
        public List<object> Data { get; set; }
        public string Message { get; set; }
    }
}
