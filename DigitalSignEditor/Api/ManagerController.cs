using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.GithubExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ManagerController : ControllerBase {
        private readonly IFileCreator fileCreator;
        private readonly ISignRepository signRepository;

        public ManagerController(ISignRepository signRepository, IFileCreator fileCreator) {
            this.signRepository = signRepository;
            this.fileCreator = fileCreator;
        }

        [HttpGet("Transfer")]
        public async Task<string> TransferSigns() {
            foreach (SignType signType in Enum.GetValues(typeof(SignType))) {
                var signs = await signRepository.ReadAsync(sr => sr.Signs.Include(s => s.Slides).Where(s => s.SignType == signType && s.Slides.Count > 0));
                var text = JsonConvert.SerializeObject(signs);
                _ = await fileCreator.CreateSharedDataFile(signType.ToString(), text);
                foreach (var slide in signs.SelectMany(s => s.Slides).Where(slide => slide.StorageItemId.HasValue)) {
                    var file = await signRepository.ReadAsync(sr => sr.StorageItems.FirstOrDefault(s => s.Id == slide.StorageItemId));
                    _ = await fileCreator.CreateImageFiles(slide.Sign.Url, file.Name, file.Data);
                    slide.AssignUrl(fileCreator.GetUrl(slide.Sign.Url, file.Name));
                    signRepository.Update(slide);
                    signRepository.Delete(file);
                }
                foreach (var sign in signs) {
                    var individualText = JsonConvert.SerializeObject(sign);
                    _ = await fileCreator.CreateDataFile(sign.Url, individualText);
                }
            }
            _ = await fileCreator.Commit();
            return "OK";
        }
    }
}