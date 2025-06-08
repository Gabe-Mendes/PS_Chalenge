using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;

namespace Questao5.Infrastructure.Swagger
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                schema.Type = "string";
                schema.Format = null;
                
                foreach (var name in Enum.GetNames(context.Type))
                {
                    var enumMemberAttribute = context.Type
                        .GetField(name)
                        ?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                        .FirstOrDefault() as EnumMemberAttribute;
                        
                    if (enumMemberAttribute?.Value != null)
                    {
                        schema.Enum.Add(new OpenApiString(enumMemberAttribute.Value));
                    }
                    else
                    {
                        var value = Enum.Parse(context.Type, name);
                        schema.Enum.Add(new OpenApiString(value.ToString()));
                    }
                }
            }
        }
    }
}