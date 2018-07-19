using cubido.OpenApi.Models.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models
{
    /// <summary>
    /// Describes a single API operation on a path.
    /// see https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md#operation-object
    /// </summary>
    [DebuggerDisplay("Name = {Name}")]
    public class Operation
    {
        // getUsers
        public string Name { get; set; }
        public string Summary { get; set; }

        // /api/user
        public string Path { get; set; }

        // Get, Post, Put
        public RequestMethod HttpMethod { get; set; }

        public List<OperationParameter> Parameters { get; set; }

        public AbstractType ResponseType { get; set; }
    }
}
