using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models
{
    [DebuggerDisplay("Name = {Name}")]
    public class Property : Variable
    {
        public string Description { get; set; }
    }
}
