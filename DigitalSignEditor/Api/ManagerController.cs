using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.GithubExport;
using DigitalSignEditor.GithubModels;
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
                var signs = await signRepository.ReadAsync(sr => sr.Signs.Include(s => s.Slides).Where(s => s.SignType == signType));
                var text = JsonConvert.SerializeObject(signs.Select(sign => new GithubSign(sign)));
                _ = await fileCreator.CreateSharedDataFile(signType.ToString(), text);
                foreach (var slide in signs.SelectMany(s => s.Slides).Where(slide => slide.StorageItemId.HasValue)) {
                    var file = await signRepository.ReadAsync(sr => sr.StorageItems.FirstOrDefault(s => s.Id == slide.StorageItemId));
                    _ = await fileCreator.CreateImageFiles(slide.Sign.Url, file.Name, file.Data);
                    slide.AssignUrl(fileCreator.GetUrl(slide.Sign.Url, file.Name),
                        fileCreator.GetFullUrl(slide.Sign.Url, file.Name),
                        fileCreator.GetCompressedUrl(slide.Sign.Url, file.Name));
                    signRepository.Update(slide);
                    signRepository.Delete(file);
                }
                await signs.ForEachAsync(sign => fileCreator.CreateDataFile(sign.Url, JsonConvert.SerializeObject(new GithubSign(sign))));
            }
            _ = await fileCreator.Commit();
            return "OK";
        }
    }
}