using System.Security.Claims;

namespace DigitalSignEditor.Helpers {
    public interface ISecurityHelper {
        bool CanAccess(ClaimsPrincipal user, int signId);
        bool IsAdmin(ClaimsPrincipal user);
        bool IsLoggedIn(ClaimsPrincipal user);
    }
}