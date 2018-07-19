using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models.Types
{
    public abstract class EnumType : AbstractType
    {
        public AbstractType Underlying { get; set; }
        internal override IEnumerable<ModelType> ModelReferences => Underlying?.ModelReferences;
    }

    public class EnumType<T> : EnumType
    {

        public List<T> Values { get; set; }

    }
}
