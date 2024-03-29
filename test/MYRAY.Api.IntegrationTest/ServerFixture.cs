using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MYRAY.Api.IntegrationTest.Helper;
using MYRAY.Business.Repositories;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Api.IntegrationTest;

public class ServerFixture<TP> : WebApplicationFactory<TP> where TP : class
{
    private HttpClient? _httpClient;

    public HttpClient ApiClient
    {
        get
        {
            if (_httpClient == null)
            {
                _httpClient = CreateClient();
            }

            return _httpClient;
        }
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        Directory.CreateDirectory("upload");//
        builder.ConfigureServices(services =>
        {
            // Use InMemoryDb
            // var descriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(DbContextOptions<MYRAYContext>));
            // var dbContextFactory = services.SingleOrDefault(
            //     d => d.ServiceType == typeof(DbContextFactory));
            // services.Remove(descriptor!);
            // services.Remove(dbContextFactory!);
                
            services.AddDbContext<MYRAYContext>(option =>
            {
                 option.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            services.AddScoped<IDbContextFactory, DbContextFactoryFake>();
            
            
            ServiceProvider sp = services.BuildServiceProvider();
            using (IServiceScope scope = sp.CreateScope())
            {
                IServiceProvider scopedService = scope.ServiceProvider;
                MYRAYContext db = scopedService.GetRequiredService<MYRAYContext>();
                // dbContextFactory.Configuration.
                try
                {
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    Utilities.InitializeDbForTests(db);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // throw;
                }
            }
        });
    }
}