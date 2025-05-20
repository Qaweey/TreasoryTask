using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core
{
    public class BaseServiceResponse<T>
    {
        public string? Code { get; set; } 
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
