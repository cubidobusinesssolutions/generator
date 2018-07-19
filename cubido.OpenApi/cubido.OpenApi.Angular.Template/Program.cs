using cubido.OpenApi.Angular.Template.Extensions;
using cubido.OpenApi.Extensions;
using cubido.OpenApi.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cubido.OpenApi.Angular.Template
{
	class Program
    {
        private static Regex QuotesToDoubleQuotes = new Regex(@"'((?:\\'|[^'\n])+)'");

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            //var targetDirectory = @"C:\Projects\Local\Cubido.ConferenceGuide.Ionic\src\app";
            var targetDirectory = @"R:\OpenApiOutput";
            Console.Write($"Converting Angular code...");

            // Open API Demo
            //await ConvertDocumentFromUrlAsync(@"http://petstore.swagger.io/v2/swagger.json", targetDirectory, new ConvertOptions()
            //{
            //    ModelsDirectoryName = "models",
            //    ServicesDirectoryName = "providers",
            //    HelpersDirectoryName = "shared"
            //});

            // Constantia Interactive
            //await ConvertDocumentFromUrlAsync(@"http://localhost:1215/swagger/v1/swagger.json", targetDirectory, new ConvertOptions()
            //{
            //    ModelsDirectoryName = "model",
            //    ServicesDirectoryName = "shared",
            //    HelpersDirectoryName = "shared"
            //});

            // ORF
            //await ConvertDocumentFromUrlAsync(@"http://localhost:5000/swagger/v1/swagger.json", targetDirectory, new ConvertOptions()
            //{
            //    ModelsDirectoryName = "shared",
            //    ServicesDirectoryName = "shared",
            //    HelpersDirectoryName = "shared"
            //});

            // Palfinger.NDS.API
            //await ConvertDocumentFromUrlAsync(@"http://localhost:1057/swagger/v1/swagger.json", targetDirectory, new ConvertOptions()
            //{
            //    BaseUrl = "api_networkdevelopmentsystem",
            //    ModelsDirectoryName = "model",
            //    ServicesDirectoryName = "services",
            //    HelpersDirectoryName = "shared"
            //});

            // Confernece Guide
            await ConvertDocumentFromUrlAsync(@"http://localhost:21785/swagger/v2/swagger.json", targetDirectory, new ConvertOptions()
            {
                ModelsDirectoryName = "models",
                ServicesDirectoryName = "core/services",
                HelpersDirectoryName = "core/util",
                DoubleQuotes = true
            });

            // Palfinger.Paldesk.Dashboard
            //await ConvertDocumentFromUrlAsync(@"https://ms-salesprojects-test.azurewebsites.net/swagger/v1/swagger.json", targetDirectory, new ConvertOptions()
            //{
            //	BaseUrl = "api_management + '/salesprojects'",
            //	ModelsDirectoryName = "shared/models",
            //	ServicesDirectoryName = "shared/services",
            //	HelpersDirectoryName = "shared"
            //});

            Console.WriteLine($"done.");

            // open folder
            Process.Start(targetDirectory);
            //Console.ReadLine();
        }

        static async Task ConvertDocumentFromUrlAsync(string swaggerUrl, string outputPath, ConvertOptions options)
        {
            var converter = await Document.FromUrlAsync(swaggerUrl);
            var modelsPath = Path.Combine(outputPath, options.ModelsDirectoryName);
            var controllersPath = Path.Combine(outputPath, options.ServicesDirectoryName);
            var helpersPath = Path.Combine(outputPath, options.HelpersDirectoryName);
            foreach (var path in new[] { modelsPath, controllersPath, helpersPath })
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
            converter.Models
                .AsParallel()
                .ForAll(async model =>
                {
                    string path = Path.Combine(modelsPath, $"{model.Name.ToLowerKebapCase()}.model.ts");
                    string content = Generated.Template.TransformText(model, options);
                    if (options.DoubleQuotes)
                    {
                        content = QuotesToDoubleQuotes.Replace(content, "\"$1\"");
                    }
                    await FileExtensions.WriteAllTextAsync(
                        path: path,
                        contents: content
                    );
                });
            converter.Controllers
                .AsParallel()
                .ForAll(async controller =>
                {
                    string path = Path.Combine(controllersPath, $"{controller.Name.ToLowerKebapCase()}.service.ts");
                    string content = Generated.Template.TransformText(controller, options);
                    if (options.DoubleQuotes)
                    {
                        content = QuotesToDoubleQuotes.Replace(content, "\"$1\"");
                    }
                    await FileExtensions.WriteAllTextAsync(
                        path: path,
                        contents: content);
                });
            var staticFiles = new[] { "ServiceHelper.ts" };
            // TODO: append header
            staticFiles
                .AsParallel()
                .ForAll(async fileName => await FileExtensions.CopyAsync(
                    sourceFileName: Path.Combine("Generated", fileName), 
                    destFileName: Path.Combine(helpersPath, StringExtensions.ToLowerKebapCase(fileName)), 
                    overwrite: true));
        }

    }
}
