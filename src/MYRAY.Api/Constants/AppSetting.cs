using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace MYRAY.Api.Constants;

public static class AppSetting
{
    public static FirebaseApp AddFireBaseAsync()
    {
        // Get Current Path
        var currentDirectory = Directory.GetCurrentDirectory();
        // Path of firebase.json
        var jsonFirebasePath = Path.Combine(currentDirectory, "Cert", "myray-dalt-firebase.json");
        // Initialize the default app
        var defaultApp = FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(jsonFirebasePath)
        });
        return defaultApp;
    }
}