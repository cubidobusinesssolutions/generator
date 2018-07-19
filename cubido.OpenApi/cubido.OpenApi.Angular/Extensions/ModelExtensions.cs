using cubido.OpenApi.Extensions;
using cubido.OpenApi.Models;
using System.Collections.Generic;

namespace cubido.OpenApi.Angular.Extensions
{
    public static class ModelExtensions
    {
        public static string GetFilename(this Model model) => $"{model.Name.ToLowerKebapCase()}.model";

        private static readonly HashSet<string> BuiltInTypes = new HashSet<string>(new[] {
            "Event",
            "File"
        });
        public static string GetImportAlias(this Model model) => BuiltInTypes.Contains(model.Name) ? $"{model.Name}Model" : model.Name;
        public static string GetImportSelector(this Model model)
        {
            var importAlias = model.GetImportAlias();
            return model.Name == importAlias ? model.Name : $"{model.Name} as {importAlias}";
        }
    }
}
