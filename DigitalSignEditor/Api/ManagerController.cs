using System;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.GithubExport;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("Clean")]
        public string CleanSigns() {
            return SignManager.DeleteSlides(signRepository).ToString();
        }

        [HttpGet("Transfer")]
        public string TransferSigns() {
            if (!ChangedHelper.HasSignChanged(signRepository)) {
                return "skipped";
            }
            Task.Run(() => {
                Console.WriteLine(SignManager.TransferSlides(signRepository, fileCreatorFactory));
            }).Forget();
            return "";
        }
    }
}