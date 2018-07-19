using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cubido.OpenApi.Models.Types;

namespace cubido.OpenApi.Models
{
    // see https://swagger.io/specification/#definitionsObject
    // see https://swagger.io/specification/#schemaObject
    [DebuggerDisplay("Name = {Name}")]
    public class Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Property> Properties { get; set; }

        public IEnumerable<Model> ImportedModels => Properties
            .Select(property => property.Type)
            .Where(type => type != null)
            .Select(type => type.ModelReferences)
            .Where(references => references != null)
            .SelectMany(references => references)
            .Select(type => type.Model)
            .Where(type => type != this)
            .Distinct();

        public bool NeedsServiceHelper()
        {
            return this.Properties.Any(p =>
            {
                var dictType = p.Type as DictionaryType;
                if (dictType != null)
                {
                    return dictType.Key is NumberType || dictType.Key is StringType;
                }
                return false;
            });
        }
    }
}
