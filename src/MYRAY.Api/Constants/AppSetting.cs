using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace MYRAY.Api.Constants;

/// <summary>
/// More app config
/// </summary>
public static class AppSetting
{
    /// <summary>
    /// Load Firebase
    /// </summary>
    public static void AddFireBaseAsync()
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
    }
}