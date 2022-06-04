using MYRAY.Api.Constants;
using MYRAY.Business;
using MYRAY.DataTier;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls($"http://localhost:8080;https://localhost:443");

builder.Services.RegisterSwaggerModule();


//
builder.Services.RegisterDataTierModule();
builder.Services.RegisterBusinessModule();
//

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
app.UseApplicationSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
