using Microsoft.Extensions.Configuration;
using Telnyx;

namespace MYRAY.Business.Helpers;

public static class SendSMSHelper
{
    public static async Task<MessagingSenderId> SendSMSTelnyx(string phoneNumber, string body, IConfiguration configuration)
    {
        var key = configuration.GetSection("TelnyxSMS").GetSection("Key").Value;
        var number = configuration.GetSection("TelnyxSMS").GetSection("PhoneNumber").Value;
        TelnyxConfiguration.SetApiKey(key);
        MessagingSenderIdService service = new MessagingSenderIdService();
        NewMessagingSenderId options = new NewMessagingSenderId
        {
            From = number, // alphanumeric sender id
            To = phoneNumber.ConvertVNPhoneNumber(),
            Text = body
        };
        MessagingSenderId messageResponse = await service.CreateAsync(options);
        return messageResponse;
    }

    
}