using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class ApiResponse<T>
    {
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
