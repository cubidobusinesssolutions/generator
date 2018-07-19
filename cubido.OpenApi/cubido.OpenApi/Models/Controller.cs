using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using cubido.OpenApi.Extensions;

namespace cubido.OpenApi.Models
{
    // see https://swagger.io/specification/#tagObject
    [DebuggerDisplay("Name = {Name}")]
    public class Controller
    {
        // Pet
        public string Name { get; set; }
        // Everything about your pets
        public string Description { get; set; }
        // http://petstore.swagger.io/v2
        public string Host { get; set; }

        public List<Operation> Operations { get; set; }

        public IEnumerable<Model> ReferencedModels => Operations
            .SelectMany(operation => operation.Parameters
                .Select(parameter => parameter.Type)
                .Append(operation.ResponseType))
            .Select(type => type?.ModelReferences)
            .Where(references => references != null)
            .SelectMany(references => references)
            .Select(type => type.Model)
            .Distinct();
    }
}
