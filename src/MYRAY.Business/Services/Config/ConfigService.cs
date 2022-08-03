using Microsoft.Extensions.Configuration;
using MYRAY.Business.DTOs.Config;
using MYRAY.Business.Repositories.Config;

namespace MYRAY.Business.Services.Config;

public class ConfigService : IConfigService
{
    private readonly IConfiguration _configuration;
    private readonly IConfigRepository _configRepository;

    public ConfigService(IConfiguration configuration,
        IConfigRepository configRepository)
    {
        _configuration = configuration;
        _configRepository = configRepository;
    }

    public ConfigDetail GetConfig()
    {
        // float priceJobPost = float.Parse(_configuration.GetSection("Money").GetSection("JobPost").Value);
        // float pricePoint = float.Parse(_configuration.GetSection("Money").GetSection("Point").Value);
        // float earnPoint = float.Parse(_configuration.GetSection("Money").GetSection("EarnPoint").Value);
        float priceJobPost = float.Parse((_configRepository.GetConfigByKey("JobPost"))!);
        float pricePoint = float.Parse((_configRepository.GetConfigByKey("Point"))!);
        float earnPoint = float.Parse((_configRepository.GetConfigByKey("EarnPoint"))!);
        var result = new ConfigDetail()
        {
            Point = (int)pricePoint,
            JobPost = priceJobPost,
            EarnPoint = (int)earnPoint
        };

        return result;
    }

    public async Task SetConfig(string key, string value)
    {
        try
        {
            await _configRepository.UpdateConfig(key, value);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}