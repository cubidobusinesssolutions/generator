using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models.Types
{
    public abstract class AbstractType
    {
        internal abstract IEnumerable<ModelType> ModelReferences { get; }
    }
}
