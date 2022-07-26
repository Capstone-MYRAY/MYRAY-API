using System.Text;
using Google.Apis.Json;
using MYRAY.DataTier.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MYRAY.Api.IntegrationTest.Helper;

public static class Utilities
{
    public static JsonSerializerSettings JsonSetting()
    {
        var setting = new JsonSerializerSettings()
        {
            ContractResolver = new NewtonsoftJsonContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        return setting;
    }
    
    public static StringContent JTokenToStringContent(this JToken obj) => new(obj.ToString(Formatting.None), Encoding.Default, "application/json");

    public static void InitializeDbForTests(MYRAYContext db)
    {
        InitializeRole(db);
        db.SaveChanges();
    }

    private static void InitializeRole(MYRAYContext db)
    {
        List<Role> listRole = new List<Role>()
        {
            new() { Id = 1, Name = "Admin", Status = 1 },
            new() { Id = 2, Name = "Moderator", Status = 1 },
            new() { Id = 3, Name = "Landowner", Status = 1 },
            new() { Id = 4, Name = "Farmer", Status = 1 },
        };
        db.Roles.AddRange(listRole);
    }

}