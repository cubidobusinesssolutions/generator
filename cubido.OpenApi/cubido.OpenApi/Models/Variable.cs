using cubido.OpenApi.Models.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models
{
    [DebuggerDisplay("Name = {Name}")]
    public class Variable
    {
        // id
        public string Name { get; set; }

        // /api/user
        public AbstractType Type { get; set; }
    }
}
