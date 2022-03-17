using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.ApiModels;
using DigitalSignEditor.Data;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SignController : ControllerBase {
        private SecurityHelper securityHelper;
        private ISignRepository signRepository;

        public SignController(ISignRepository signRepository, SecurityHelper securityHelper) {
            this.signRepository = signRepository;
            this.securityHelper = securityHelper;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<SignGroup> GetAllSigns() {
            var signs = await signRepository.ReadAsync(rep => rep.Signs.Where(s => s.IsActive));
            return new SignGroup(signs.ToList());
        }

        [HttpGet("Get/{id}")]
        public async Task<SignInformation> GetSign(int id) {
            if (!securityHelper.CanAccess(User, id)) {
                return new SignInformation();
            }
            var sign = await signRepository.ReadAsync(rep => rep.Signs.Include(s => s.Slides).FirstOrDefault(s => s.IsActive && s.Id == id));
            return new SignInformation(sign);
        }

        [HttpGet("Get")]
        public async Task<SignGroup> GetSigns() {
            if (!securityHelper.IsLoggedIn(User)) {
                return new SignGroup();
            }
            if (securityHelper.IsAdmin(User)) {
                return await GetAllSigns();
            }
            var signs = await signRepository.ReadAsync(rep => rep.Signs.Include(s => s.SignPermissions).Where(s => s.IsActive && s.SignPermissions.Select(sp => sp.Name).Contains(User.Identity.Name)));
            return new SignGroup(signs.ToList());
        }
    }
}