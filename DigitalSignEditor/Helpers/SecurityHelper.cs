using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DigitalSignEditor.Data;

namespace DigitalSignEditor.Helpers {

    public class SecurityHelper {

        public SecurityHelper(ISignRepository signRepository, string adminList) {
            admin = adminList.Split(";").ToList();
            this.signRepository = signRepository;
        }

        private List<string> admin { get; set; }

        private ISignRepository signRepository { get; set; }

        public bool CanAccess(ClaimsPrincipal user, int signId) {
            if (!IsLoggedIn(user)) {
                return false;
            }
            if (IsAdmin(user)) {
                return true;
            }
            return signRepository.Read(rep => rep.SignPermissions.Any(sp => sp.SignId == signId && sp.Name == user.Identity.Name));
        }

        public bool IsAdmin(ClaimsPrincipal user) {
            if (!IsLoggedIn(user)) {
                return false;
            }
            return admin.Contains(user.Identity.Name);
        }

        public bool IsLoggedIn(ClaimsPrincipal user) {
            return user != null && user.Identity != null && !string.IsNullOrWhiteSpace(user.Identity.Name);
        }
    }
}