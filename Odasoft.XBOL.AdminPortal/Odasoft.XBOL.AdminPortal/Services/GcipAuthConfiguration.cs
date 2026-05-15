using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Odasoft.XBOL.AdminPortal.Services;

public static class GcipAuthConfiguration
{
    public static FirebaseApp InitializeFirebaseApp(GcipAuthOptions options)
    {
        try
        {
            return FirebaseApp.DefaultInstance ?? FirebaseApp.Create(BuildAppOptions(options));
        }
        catch (ArgumentException)
        {
            return FirebaseApp.DefaultInstance!;
        }
    }

    public static AppOptions BuildAppOptions(GcipAuthOptions options)
    {
        var appOptions = new AppOptions
        {
            Credential = !string.IsNullOrWhiteSpace(options.ServiceAccountJson)
#pragma warning disable CS0618
                ? GoogleCredential.FromJson(options.ServiceAccountJson)
                : GoogleCredential.FromFile(options.ServiceAccountJsonPath!)
#pragma warning restore CS0618
        };

        if (!string.IsNullOrWhiteSpace(options.ProjectId))
            appOptions.ProjectId = options.ProjectId;

        return appOptions;
    }
}
