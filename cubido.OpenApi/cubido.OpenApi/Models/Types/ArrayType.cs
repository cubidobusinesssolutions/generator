using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models.Types
{
    public class ArrayType : AbstractType
    {
        public AbstractType Inner { get; set; }

        internal override IEnumerable<ModelType> ModelReferences => Inner?.ModelReferences;
    }
}
