using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models.Types
{
    [DebuggerDisplay("Model = {Model?.Name}")]
    public class ModelType : AbstractType
    {
        public Model Model { get; set; }

        // TODO: make internal
        /// <summary>Temporary reference to original schema definition.</summary>
        internal JsonSchema4 Schema { get; set; }

        internal override IEnumerable<ModelType> ModelReferences => new[] { this };
    }
}
