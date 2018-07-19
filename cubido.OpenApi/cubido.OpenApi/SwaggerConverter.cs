using cubido.OpenApi.Extensions;
using cubido.OpenApi.Models;
using cubido.OpenApi.Models.Types;
using NJsonSchema;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cubido.OpenApi.Angular
{
	// Swagger.json specification see http://swagger.io/specification
	// NSwag source code, see https://github.com/NSwag/NSwag/tree/master/src/NSwag.Core

	public class SwaggerConverter
    {
        protected readonly SwaggerDocument Document; // initialized in constructor

        [Obsolete("Use AbstractType machinery instead.")]
        protected readonly TypeScriptTypeResolver TypeResolver; // initialized in constructor

        internal SwaggerConverter(SwaggerDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        internal void ApplyModelReferences(IDictionary<JsonSchema4, Model> models)
        {
            foreach (var modelType in _modelTypes)
            {
                modelType.Model = models[modelType.Schema];
                modelType.Schema = null;
            }
        }

        /// <summary>
        /// Keeps track of all model types in order to replace <see cref="ModelType.Schema" /> by <see cref="ModelType.Model" /> afterwards.
        /// </summary>
        protected readonly List<ModelType> _modelTypes = new List<ModelType>();

        internal Model CreateModel(string name, JsonSchema4 definition)
        {
            return new Model()
            {
                Name = name,
                Description = definition.Description,
                Properties = definition.ActualProperties.Values.Select(property => new Property
                {
                    Name = property.Name,
                    Description = property.Description,
                    Type = GetType(property)
                }).ToList()
            };
        }

        internal IEnumerable<Controller> CreateControllers()
        {
            return Document.Paths
                .SelectMany(path => path.Value
                    .Select(operation => new
                    {
                        Tag = operation.Value.Tags.First(), // dismiss further tags?!
                        Operation = CreateOperation(path.Key.ToString(), operation.Key, operation.Value)
                    }))
                .GroupBy(a => a.Tag, a => a.Operation, (tag, operations1) => new { Tag = tag, Operations = operations1 })
                .LeftJoin(Document.Tags ?? Enumerable.Empty<SwaggerTag>(), a => a.Tag, tag => tag.Name, (a, tag) => new Controller()
                {
                    Name = a.Tag,
                    Description = tag?.Description,
                    Host = $"{Document.Schemes.Select(scheme => scheme.ToString().ToLower()).LastOrDefault() ?? "http"}://{Document.Host}{(Document.BasePath == "/" ? "" : Document.BasePath)}",
                    Operations = a.Operations.ToList()
                });
        }

        internal Operation CreateOperation(string path, SwaggerOperationMethod httpMethod, SwaggerOperation operation)
        {
            return new Operation()
            {
                Name = operation.OperationId,
                Summary = operation.Summary,
                Path = path,
                HttpMethod = GetHttpMethod(httpMethod),
                Parameters = operation.ActualParameters.Select(parameter => new OperationParameter()
                {
                    Name = parameter.Name,
					Description = parameter.Description,
                    Location = GetOperationParameterLocation(parameter.Kind),
                    Type = GetType(parameter.Kind == SwaggerParameterKind.Body ? parameter.Schema : parameter),
                    IsRequired = parameter.IsRequired
                }).ToList(),
                ResponseType = operation.Responses
                    .Where(response => response.Key == "200")
                    .Select(response => response.Value.Schema)
                    .Select(schema => schema == null ? null : GetType(schema))
                    .FirstOrDefault()
            };
        }

        private static RequestMethod GetHttpMethod(SwaggerOperationMethod method)
        {
            switch (method)
            {
                case SwaggerOperationMethod.Undefined: throw new ArgumentException(nameof(method));
                case SwaggerOperationMethod.Get: return RequestMethod.Get;
                case SwaggerOperationMethod.Post: return RequestMethod.Post;
                case SwaggerOperationMethod.Put: return RequestMethod.Put;
                case SwaggerOperationMethod.Delete: return RequestMethod.Delete;
                case SwaggerOperationMethod.Options: return RequestMethod.Options;
                case SwaggerOperationMethod.Head: return RequestMethod.Head;
                case SwaggerOperationMethod.Patch: return RequestMethod.Patch;
                default: throw new NotImplementedException();
            }
        }

        private static OperationParameterLocation GetOperationParameterLocation(SwaggerParameterKind kind)
        {
            switch (kind)
            {
                case SwaggerParameterKind.Undefined: throw new ArgumentException(nameof(kind));
                case SwaggerParameterKind.Body: return OperationParameterLocation.Body;
                case SwaggerParameterKind.Query: return OperationParameterLocation.Query;
                case SwaggerParameterKind.Path: return OperationParameterLocation.Path;
                case SwaggerParameterKind.Header: return OperationParameterLocation.Header;
                case SwaggerParameterKind.FormData: return OperationParameterLocation.FormData;
                default: throw new NotImplementedException();
            }
        }

        private static bool IsModel(JsonSchema4 schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));

            if (schema.HasReference) return true;
            if (schema.Type == JsonObjectType.Array) return IsModel(schema.Item);
            return false;
        }

        protected AbstractType GetType(JsonSchema4 schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));

            // enums
            if (schema.Enumeration.Any())
            {
                // TODO: create own Enum classes
                switch (schema.Type)
                {
                    case JsonObjectType.Integer: return new EnumType<int>()
                    {
                        Underlying = new NumberType(),
                        Values = schema.Enumeration.Select(item => Convert.ToInt32(item)).ToList()
                    };
                    case JsonObjectType.String: return new EnumType<string>()
                    {
                        Underlying = new StringType(),
                        Values = schema.Enumeration.Cast<string>().ToList()
                    };
                    default: throw new NotImplementedException($"Enum type \"{schema.Type}\" is not implemented.");
                }
            }

            // arrays
            if (schema.Type == JsonObjectType.Array) return new ArrayType() { Inner = GetType(schema.Item) };

            // models
            if (schema.HasReference)
            {
                var result = new ModelType() { Schema = schema.Reference };
                this._modelTypes.Add(result);
                return result;
            }

            // remaining types
            switch (schema.Type)
            {
                case JsonObjectType.Boolean: return new BooleanType();
                case JsonObjectType.Integer: return new NumberType();
                case JsonObjectType.Number: return new NumberType();
                case JsonObjectType.String: return schema.Format == "date-time"
                    ? (AbstractType)new DateTimeType()
                    : new StringType();
                case JsonObjectType.Object: return schema.AdditionalPropertiesSchema == null
                    ? null
                    : new DictionaryType() { Key = new StringType(), Value = GetType(schema.AdditionalPropertiesSchema) };
                case JsonObjectType.File: return new FileType();
                case JsonObjectType.Null: return null;
            }

            throw new NotImplementedException($"Type {schema.Type} is not implemented");
        }
    }
}
