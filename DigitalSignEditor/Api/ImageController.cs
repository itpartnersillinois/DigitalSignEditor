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
        public async Task<int> AddImage([FromForm] IFormFile file, [FromForm] int id) {
            var sign = await signRepository.ReadAsync(rep => rep.SignItems.FirstOrDefault(s => s.Id == id));
            if (sign == null || !securityHelper.CanAccess(User, sign.SignId)) {
                return default;
            }
            using (var ms = new MemoryStream()) {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var storageId = await signRepository.CreateAsync(new StorageItem {
                    IsActive = true,
                    LastUpdated = DateTime.Now,
                    Name = file.FileName,
                    Data = fileBytes
                });
                sign.StorageItemId = storageId;
                return await signRepository.UpdateAsync(sign);
            }
        }
    }
}