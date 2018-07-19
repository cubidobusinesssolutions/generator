using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models.Types
{
    public class DictionaryType : AbstractType
    {
        public AbstractType Key { get; set; }
        public AbstractType Value { get; set; }
        internal override IEnumerable<ModelType> ModelReferences => new[] { Key?.ModelReferences, Value?.ModelReferences }.Where(references => references != null).SelectMany(references => references);
    }
}
