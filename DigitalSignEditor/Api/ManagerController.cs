using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.ApiModels;
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

        [HttpGet("CleanCommit")]
        public string CleanCommit() => SignManager.RunCommit(fileCreatorFactory).ToString();

        [HttpGet("Clean")]
        public string CleanSigns() => SignManager.DeleteSlides(signRepository).ToString();

        [HttpGet("GetQueue")]
        public async Task<IEnumerable<QueueItem>> GetQueue() {
            var changeIndicators = (await signRepository.ReadAsync(rep => rep.ChangeIndicators.OrderByDescending(ci => ci.LastUpdated).Take(30))).ToList();
            return changeIndicators.Select(ci => new QueueItem { Name = ci.Name, IsActive = ci.IsActive, LastUpdated = ci.LastUpdated });
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

        [HttpGet("ForceTransfer")]
        public string TransferSignsForce() {
            Task.Run(() => {
                Console.WriteLine(SignManager.TransferSlides(signRepository, fileCreatorFactory));
            }).Forget();
            return "forced transfer";
        }
    }
}