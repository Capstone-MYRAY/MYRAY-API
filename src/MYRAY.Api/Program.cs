using System.Text.Json.Serialization;
using Google.Apis.Json;
using MYRAY.Api.Constants;
using MYRAY.Business;
using MYRAY.DataTier;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
      o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls($"http://localhost:8080;https://localhost:443");


AppSetting.AddFireBaseAsync();
builder.Services.AddCors();
builder.Services.RegisterSecurityModule(builder.Configuration);
builder.Services.RegisterSwaggerModule();


//
builder.Services.RegisterDataTierModule();
builder.Services.RegisterBusinessModule();
//

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(c
    => c.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod())
        ;

app.UseApplicationSwagger();

app.UseHttpsRedirection();

app.UseApplicationSecurity();

app.MapControllers();

app.Run();
