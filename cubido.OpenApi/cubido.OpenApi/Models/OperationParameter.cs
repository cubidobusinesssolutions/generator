using System.Diagnostics;

namespace cubido.OpenApi.Models
{
    /// <summary>
    /// Describes a single operation parameter.
    /// see http://swagger.io/specification/#parameterObject
    /// </summary>
    [DebuggerDisplay("Name = {Name}")]
    public class OperationParameter : Variable
    {
        /// <summary>The location of the parameter.</summary>
        public OperationParameterLocation Location { get; set; }

        /// <summary>A brief description of the parameter.</summary>
        public string Description { get; set; }
        public bool IsRequired { get; set; }
    }
}
