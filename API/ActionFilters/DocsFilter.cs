using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.ActionFilters
{
    public class DocsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            ControllerActionDescriptor ActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (ActionDescriptor != null)
            {
                bool allowAll = ActionDescriptor.EndpointMetadata.OfType<AllowAllAttribute>().Any();
                bool allowAnonymous = ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

                if (!allowAll)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = HeadersConstants.Validator,
                        In = ParameterLocation.Header,
                        Required = false,
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        }
                    });
                }
                if (!allowAnonymous && !allowAll)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = HeadersConstants.AuthorizationToken,
                        In = ParameterLocation.Header,
                        Required = true,
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        }
                    });
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeadersConstants.Culture,
                    In = ParameterLocation.Header,
                    Description = "en | ar",
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeadersConstants.Theme,
                    In = ParameterLocation.Header,
                    Description = "light | dark",
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                });
            }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class SwaggerIgnoreAttribute : Attribute
    {

    }

    public class SwaggerSkipPropertyFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            IEnumerable<PropertyInfo> skipProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerIgnoreAttribute>() != null);

            foreach (PropertyInfo skipProperty in skipProperties)
            {
                string propertyToSkip = schema.Properties.Keys.SingleOrDefault(x => string.Equals(x, skipProperty.Name, StringComparison.OrdinalIgnoreCase));

                if (propertyToSkip != null)
                {
                    _ = schema.Properties.Remove(propertyToSkip);
                }
            }
        }
    }
}
