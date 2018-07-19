using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models.Types
{
    // see https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md#data-types
    [DebuggerDisplay("DateTime")]
    public class DateTimeType : AbstractType
    {
        internal override IEnumerable<ModelType> ModelReferences => null;
    }
}
