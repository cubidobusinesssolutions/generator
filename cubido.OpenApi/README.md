# Introduction

This project creates frontend model and service classes from an OpenAPI specification (formerly known as Swagger specification). Read more about how to generate such an OpenAPI specification in [../cubido.web/README.md](cubido.web\README.md). 

## Deprecation Warning

This project is no longer maintained in favour of [Swagger codegen](https://github.com/swagger-api/swagger-codegen).

## Angular
Specify the url(s) to your OpenAPI specification(s) in `cubido.OpenApi.Angular.Template`.
```cs
static async Task MainAsync(string[] args)
{
    await ConvertDocumentFromUrlAsync(@"http://petstore.swagger.io/v2/swagger.json", targetDirectory, new ConvertOptions()
    {
        ModelsDirectoryName = "models",
        ServicesDirectoryName = "providers",
        HelpersDirectoryName = "shared"
    });
};
```
Run the template project and enjoy the generated files located in `src`.
