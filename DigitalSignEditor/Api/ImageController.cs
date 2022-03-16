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
            var sign = await signRepository.ReadAsync(rep => rep.Signs.FirstOrDefault(s => s.Id == id));
            if (sign == null || !securityHelper.CanAccess(User, sign.Id)) {
                return default;
            }
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            if (!ImageHelper.IsImageValid(fileBytes, sign.MinimumWidth, sign.MinimumHeight, sign.RatioWidth, sign.RatioHeight)) {
                return default;
            }
            var storage = new StorageItem {
                IsActive = true,
                LastUpdated = DateTime.Now,
                Name = file.FileName,
                Data = fileBytes
            };
            await signRepository.CreateAsync(storage);
            var totalItems = await signRepository.ReadAsync(rep => rep.Slides.Count(s => s.SignId == sign.Id));
            return await signRepository.CreateAsync(new Slide {
                IsActive = true,
                LastUpdated = DateTime.Now,
                Name = file.FileName,
                SignId = sign.Id,
                EndDate = null,
                StartDate = null,
                Option = SlideType.Image,
                Order = totalItems + 1,
                StorageItemId = storage.Id
            });
        }
    }
}