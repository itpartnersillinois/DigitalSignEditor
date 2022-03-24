using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.GithubExport;
using DigitalSignEditor.GithubModels;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ManagerController : ControllerBase {
        private readonly IFileCreatorFactory fileCreatorFactory;
        private readonly ISignRepository signRepository;

        public ManagerController(ISignRepository signRepository, IFileCreatorFactory fileCreatorFactory) {
            this.signRepository = signRepository;
            this.fileCreatorFactory = fileCreatorFactory;
        }

        [HttpGet("Transfer")]
        public string TransferSigns() {
            Task.Run(() => {
                var returnValue = new StringBuilder();
                foreach (SignType signType in Enum.GetValues(typeof(SignType))) {
                    var signs = signRepository.Read(sr => sr.Signs.Include(s => s.Slides).Where(s => s.SignType == signType));
                    _ = returnValue.Append(" Slide " + signType.ToString() + ". ");
                    foreach (var slide in signs.SelectMany(s => s.Slides).Where(slide => slide.StorageItemId.HasValue)) {
                        var signUrl = signs.First(s => s.Id == slide.SignId).Url;
                        var file = signRepository.Read(sr => sr.StorageItems.FirstOrDefault(s => s.Id == slide.StorageItemId));
                        _ = fileCreatorFactory.Create().CreateImageFiles(signUrl, file.Name, file.Data);
                        slide.AssignUrl(fileCreatorFactory.Create().GetUrl(signUrl, file.Name),
                            fileCreatorFactory.Create().GetFullUrl(signUrl, file.Name),
                            fileCreatorFactory.Create().GetCompressedUrl(signUrl, file.Name));
                        _ = signRepository.Update(slide);
                        _ = signRepository.Delete(file);
                        returnValue.Append(" - " + file.Name);
                    }
                    _ = fileCreatorFactory.Create().CreateSharedDataFile(signType.ToString(), JsonConvert.SerializeObject(signs.Select(sign => new GithubSign(sign))));
                    _ = returnValue.Append(" - Shared file ");
                    foreach (var sign in signs) {
                        _ = fileCreatorFactory.Create().CreateDataFile(sign.Url, JsonConvert.SerializeObject(new GithubSign(sign)));
                        _ = returnValue.Append(" - " + sign.Url);
                    }
                }
                _ = fileCreatorFactory.Create().Commit();
                returnValue.Append(" - Commit.");
                Console.WriteLine(returnValue.ToString());
            }).Forget();
            return "";
        }
    }
}