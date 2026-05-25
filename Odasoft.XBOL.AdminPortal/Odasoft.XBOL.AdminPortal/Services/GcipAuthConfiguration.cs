using FirebaseAdmin;

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
            Credential = GoogleServiceAccountCredentialFactory.Create(
                options.ServiceAccountJson,
                options.ServiceAccountJsonPath)
        };

        if (!string.IsNullOrWhiteSpace(options.ProjectId))
        {
            appOptions.ProjectId = options.ProjectId;
        }

        return appOptions;
    }
}
