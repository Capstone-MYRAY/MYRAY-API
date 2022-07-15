using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

namespace MYRAY.Api.Constants;

/// <summary>
/// Configuration of Swagger 
/// </summary>
public static class SwaggerSetting
{
    /// <summary>
    /// Service class for Swagger.
    /// </summary>
    /// <param name="services">Service container form Program</param>
    /// <returns>IServiceCollection</returns>
    public static void RegisterSwaggerModule(this IServiceCollection services)
    {
        //--Add API Versioning to as service to your project.
        services.AddApiVersioning(config =>
        {
            //--Specify the default API Version
            config.DefaultApiVersion = ApiVersion.Default;
            
            //--If no set API, use default API version
            config.AssumeDefaultVersionWhenUnspecified = true;

            //--Show API version support
            config.ReportApiVersions = true;
            
            //--Support different versioning ways
            // config.ApiVersionReader = ApiVersionReader.Combine(
            //     new QueryStringApiVersionReader("api-version"),
            //     new HeaderApiVersionReader("X-Version"),
            //     new MediaTypeApiVersionReader("ver"));
            
        });
        
        // Config versioning
        services.AddVersionedApiExplorer(options =>
        {
            //--Format of versioning (v1.0)
            options.GroupNameFormat = "'v'VVV";
            
            //-- Support Endpoints temlate 'api/v{version}/[controller]
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(c =>
        {
            // Set Swagger Description
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Connecting Landowner And Farmer System",
                Version = "v1",
                Description = "MYRAY Endpoints",
                Contact = new OpenApiContact()
                {
                    Name = "Forest",
                    Email = "forest.tl112@gmail.com"
                }
            });
          
            
            c.DocInclusionPredicate((_, _) => true);
            
            //--Get Assembly Name Combine extension xml 
            var xmlFile = $"{typeof(Program).GetTypeInfo().Assembly.GetName().Name}.xml";
            //--Combine Base Path to xml file name
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //--Load comment from xml into Swagger UI
            c.IncludeXmlComments(xmlPath);
            
            c.DescribeAllParametersInCamelCase();
            
            var jwtSecuritySchema = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_Only_** your token on textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecuritySchema);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {jwtSecuritySchema, Array.Empty<string>()}
            });

        });

        services.AddSwaggerGenNewtonsoftSupport();
    }

    /// <summary>
    /// Config request Swagger
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static void UseApplicationSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "{documentName}/api-docs";
        });

        //--Set Router Endpoint Swagger
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/api-docs", "MYRAY WebAPI V1");
            // c.SwaggerEndpoint("v2/api-docs", "CCFRMS WebAPI V2");
            c.RoutePrefix = String.Empty;
        });
    }

}