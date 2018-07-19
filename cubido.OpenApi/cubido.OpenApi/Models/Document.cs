using cubido.OpenApi.Angular;
using NSwag;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cubido.OpenApi.Models
{
	/// <summary>
	/// This is the root document object for the API specification. 
	/// see https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md#swagger-object
	/// </summary>
	public class Document
    {
        public IEnumerable<Model> Models { get; set; }

        public IEnumerable<Controller> Controllers { get; set; }


        public static async Task<Document> FromUrlAsync(string url)
        {
            var document = await SwaggerDocument.FromUrlAsync(url);
            var converter = new SwaggerConverter(document);
            var models = document.Definitions.ToDictionary(definition => definition.Value, definition => converter.CreateModel(definition.Key, definition.Value));
            var controllers = converter.CreateControllers().ToList();
            converter.ApplyModelReferences(models);
            return new Document { Models = models.Values.ToList(), Controllers = controllers };
        }

    }
}
