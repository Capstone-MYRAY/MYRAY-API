using FirebaseAdmin.Messaging;

namespace MYRAY.Business.Services.Notification;

public static class PushNotification
{
    public static async Task SendMessage(string uid, string title, string content, Dictionary<string, string> dataSend)
    {
        try
        {
            var topic = uid;

            var message = new FirebaseAdmin.Messaging.Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = content
                },
                Android = new AndroidConfig()
                {
                    Notification = new AndroidNotification
                    {
                        Icon = "stock_ticker_update",
                        Color = "#f45342",
                        Sound = "default"
                    }
                },
                // Data = new Dictionary<string, string>()
                // {
                //     {"title", title},
                //     {"content", content}
                // },
                Data = dataSend,
                Topic = topic
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

          
    }
}
