using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : Controller {
        private ISecurityHelper securityHelper;
        private ISignRepository signRepository;

        public SecurityController(ISignRepository signRepository, ISecurityHelper securityHelper) {
            this.signRepository = signRepository;
            this.securityHelper = securityHelper;
        }

        [HttpPost("Add")]
        public async Task<int> AddSecurity([FromBody] dynamic json) {
            int id = json.id;
            string name = ((string) json.name).EndsWith("@illinois.edu") ? json.name : json.name + "@illinois.edu";
            if (!securityHelper.CanAccess(User, id)) {
                return 0;
            }
            var newUser = new SignPermission {
                Name = name,
                IsActive = true,
                LastUpdated = DateTime.Now,
                SignId = id
            };
            _ = await signRepository.CreateAsync(newUser);
            return newUser.Id;
        }

        [HttpPost("Delete")]
        public async Task<int> DeleteSecurity([FromBody] dynamic json) {
            int id = json.id;
            string name = json.name;
            if (!securityHelper.CanAccess(User, id)) {
                return 0;
            }
            var permission = await signRepository.ReadAsync(rep => rep.SignPermissions.FirstOrDefault(sp => sp.SignId == id && sp.Name == name));
            if (permission == null) {
                return 0;
            }
            _ = await signRepository.DeleteAsync(permission);
            return permission.Id;
        }

        [HttpGet("Get/{id}")]
        public async Task<List<string>> GetSecurity(int id) {
            if (!securityHelper.IsLoggedIn(User)) {
                return new List<string>();
            }
            var permissions = await signRepository.ReadAsync(rep => rep.SignPermissions.Where(sp => sp.SignId == id).Select(sp => sp.Name));
            return permissions.ToList();
        }

        [HttpGet("IsAdmin")]
        public bool IsAdmin() => securityHelper.IsAdmin(User);
    }
}