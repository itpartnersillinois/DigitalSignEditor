using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {
        private readonly SecurityHelper securityHelper;
        private readonly ISignRepository signRepository;

        public ImageController(ISignRepository signRepository, SecurityHelper securityHelper) {
            this.signRepository = signRepository;
            this.securityHelper = securityHelper;
        }

        [HttpPost("Add")]
        public async Task<int> AddImageSlide([FromForm] IFormFile file, [FromForm] int id) {
            if (!securityHelper.CanAccess(User, id)) {
                return default;
            }
            using (var ms = new MemoryStream()) {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var storage = new StorageItem {
                    IsActive = true,
                    LastUpdated = DateTime.Now,
                    Name = file.FileName,
                    Data = fileBytes
                };
                await signRepository.CreateAsync(storage);
                var totalItems = await signRepository.ReadAsync(rep => rep.SignItems.Count(s => s.SignId == id));
                return await signRepository.CreateAsync(new SignItem {
                    IsActive = true,
                    LastUpdated = DateTime.Now,
                    Name = file.FileName,
                    SignId = id,
                    EndDate = null,
                    StartDate = null,
                    Option = SignType.Image,
                    Order = totalItems + 1,
                    StorageItemId = storage.Id
                });
            }
        }
    }
}