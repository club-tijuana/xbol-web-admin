namespace Odasoft.XBOL.AdminPortal.Helpers;

public static class RegisterInvitationErrorMessages
{
    public static string? GetResourceKey(string? problemType)
    {
        return problemType switch
        {
            "invitation_required" => "InvitationTokenMissing",
            "invalid_invitation" => "InvitationTokenInvalid",
            "expired_invitation" => "InvitationTokenExpired",
            "revoked_invitation" => "InvitationTokenRevoked",
            "accepted_invitation" => "InvitationTokenAccepted",
            "email_mismatch" => "InvitationEmailMismatch",
            _ => null
        };
    }
}
