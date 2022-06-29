using Google.Apis.Json;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using MYRAY.Api.Constants;
using MYRAY.Api.Cron;
using MYRAY.Api.Hubs;
using MYRAY.Business;
using MYRAY.DataTier;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddCors(o =>
// {
//     o.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder
//         .SetIsOriginAllowedToAllowWildcardSubdomains()
//         .SetIsOriginAllowed(_ => true)
//         .AllowAnyMethod()
//         .AllowAnyHeader()
//         .AllowCredentials());
// });

builder.Services.AddSignalR();
builder.Services.Configure<IISServerOptions>(p => { p.MaxRequestBodySize = int.MaxValue; });
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.ValueCountLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = long.MaxValue;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

//-- Set controller name lowercase
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        //      x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        o.SerializerSettings.ContractResolver = new NewtonsoftJsonContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };
        o.SerializerSettings.Converters.Add(new StringEnumConverter()
        {
            AllowIntegerValues = true
        });
        o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls("http://localhost:8080;https://localhost:443");

AppSetting.AddFireBaseAsync();
//builder.Services.AddCors();
builder.Services.RegisterSecurityModule(builder.Configuration);
builder.Services.RegisterSwaggerModule();
//
builder.Services.RegisterDataTierModule();
builder.Services.RegisterBusinessModule();
//
builder.Services.RegisterQuartz();
var app = builder.Build();

Directory.CreateDirectory("upload");
app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "upload")),
    RequestPath = "/upload",
    EnableDirectoryBrowsing = true,
    StaticFileOptions = { OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    }}
});

// Configure the HTTP request pipeline.
app.UseCors(cor => cor
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .SetIsOriginAllowed(_ => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseApplicationSwagger();

app.UseHttpsRedirection();

app.UseApplicationSecurity();

app.MapHub<ChatHub>("/chatHub");

app.MapControllers();

// SchedulerTask.StartAsync().GetAwaiter().GetResult();
app.Run();
