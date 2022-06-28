using MYRAY.Business.DTOs.Config;

namespace MYRAY.Business.Services.Config;

public interface IConfigService
{
    ConfigDetail GetConfig();
    
}