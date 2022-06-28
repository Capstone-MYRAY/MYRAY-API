using Microsoft.Extensions.Configuration;
using MYRAY.Business.DTOs.Config;

namespace MYRAY.Business.Services.Config;

public class ConfigService : IConfigService
{
    private readonly IConfiguration _configuration;

    public ConfigService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ConfigDetail GetConfig()
    {
        float priceJobPost = float.Parse(_configuration.GetSection("Money").GetSection("JobPost").Value);
        float pricePoint = float.Parse(_configuration.GetSection("Money").GetSection("Point").Value);
        float earnPoint = float.Parse(_configuration.GetSection("Money").GetSection("EarnPoint").Value);
        var result = new ConfigDetail()
        {
            Point = (int)pricePoint,
            JobPost = priceJobPost,
            EarnPoint = (int)earnPoint
        };

        return result;
    }
}