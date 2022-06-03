var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls($"http://localhost:8080;https://localhost:443");
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "{documentName}/api-docs";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/api-docs", "MYRAY-API V1");
        c.RoutePrefix = string.Empty;
    });
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
