using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.ApiModels;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : Controller {
        private const int signPermissionId = 1;
        private readonly ISignRepository signRepository;
        private ISecurityHelper securityHelper;
        // first sign is the College of Education, so we are using this sign ID to manage permissions for this screen

        public DonorController(ISignRepository signRepository, ISecurityHelper securityHelper) {
            this.signRepository = signRepository;
            this.securityHelper = securityHelper;
        }

        [HttpGet("Permission")]
        public bool CheckPermission() => securityHelper.CanAccess(User, signPermissionId);

        [HttpGet("GetAll")]
        [AllowAnonymous]
        [DisableCors]
        public async Task<List<DonorInformation>> GetAll() {
            var donors = await signRepository.ReadAsync(sr => sr.Donors.Where(d => d.IsActive).OrderBy(d => d.Name).Select(d => new DonorInformation(d, false)));
            return donors.ToList();
        }

        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        [DisableCors]
        public async Task<DonorInformation> GetById(int id, bool useExternal = false) {
            var donor = await signRepository.ReadAsync(sr => sr.Donors.FirstOrDefault(d => d.Id == id));
            return donor == null ? new DonorInformation() : new DonorInformation(donor, useExternal);
        }

        [HttpGet("Get/{name}")]
        [AllowAnonymous]
        [DisableCors]
        public async Task<DonorInformation> GetByName(string name, bool useExternal = false) {
            var donor = await signRepository.ReadAsync(sr => sr.Donors.FirstOrDefault(d => d.Name == name));
            return donor == null ? new DonorInformation() : new DonorInformation(donor, useExternal);
        }

        [HttpGet("GetExternalById/{id}")]
        [AllowAnonymous]
        [DisableCors]
        public async Task<DonorInformation> GetExternalById(int id) => await GetById(id, true);

        [HttpGet("GetExternal/{name}")]
        [AllowAnonymous]
        [DisableCors]
        public async Task<DonorInformation> GetExternalByName(string name) => await GetByName(name, true);

        [HttpGet("GetImage/{name}")]
        [AllowAnonymous]
        [DisableCors]
        public async Task<byte[]> GetImage(string name) {
            var donor = await signRepository.ReadAsync(sr => sr.Donors.FirstOrDefault(d => d.Name == name));
            return donor == null ? new byte[0] : donor.Image;
        }

        [HttpGet("GetRandom/{number}")]
        [AllowAnonymous]
        [DisableCors]
        public async Task<List<DonorInformation>> GetRandom(int number) {
            var donors = await signRepository.ReadAsync(sr => sr.Donors.Where(d => d.IsActive).OrderBy(d => Guid.NewGuid().ToString()).Take(number).OrderBy(d => d.Name).Select(d => new DonorInformation(d, true)));
            return donors.ToList();
        }

        [HttpPost("RemoveImage")]
        public async Task<int> RemoveImage([FromForm] int id) {
            if (!securityHelper.CanAccess(User, signPermissionId)) {
                return 0;
            }
            var donor = await signRepository.ReadAsync(sr => sr.Donors.FirstOrDefault(d => d.Id == id));
            if (donor != null) {
                donor.Image = null;
                _ = await signRepository.UpdateAsync(donor);
            }
            return id;
        }

        [HttpPost("Save")]
        [DisableCors]
        public async Task<int> Save([FromBody] dynamic json) {
            if (!securityHelper.CanAccess(User, signPermissionId)) {
                return 0;
            }
            int id = int.Parse(json.id.ToString());
            string name = json.name.ToString();
            string personName = json.personname.ToString();
            string description = json.description.ToString();
            bool isActive = bool.Parse(json.isactive.ToString());
            var donor = await signRepository.ReadAsync(sr => sr.Donors.FirstOrDefault(d => d.Id == id));
            if (donor == null) {
                var donorNew = new Donor {
                    Name = name,
                    LastUpdated = DateTime.Now,
                    Description = description,
                    PersonName = personName,
                    IsActive = isActive
                };
                _ = await signRepository.CreateAsync(donorNew);
                return donorNew.Id;
            } else {
                donor.LastUpdated = DateTime.Now;
                donor.Name = name;
                donor.Description = description;
                donor.PersonName = personName;
                donor.IsActive = isActive;
                _ = await signRepository.UpdateAsync(donor);
            }
            return donor.Id;
        }

        [HttpPost("SaveImage")]
        public async Task<int> SaveImage([FromForm] IFormFile file, [FromForm] int id) {
            if (!securityHelper.CanAccess(User, signPermissionId)) {
                return 0;
            }
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var donor = await signRepository.ReadAsync(sr => sr.Donors.FirstOrDefault(d => d.Id == id));
            if (donor != null) {
                donor.Image = ms.ToArray();
                _ = await signRepository.UpdateAsync(donor);
            }
            return id;
        }
    }
}