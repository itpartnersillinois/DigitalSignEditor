using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.GithubExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("TransferSigns")]
        public async Task<string> TransferSigns() {
            foreach (SignType signType in Enum.GetValues(typeof(SignType))) {
                var signs = await signRepository.ReadAsync(sr => sr.Signs.Include(s => s.Slides).Where(s => s.SignType == signType));
                // create json
                // save json in github
                foreach (var slide in signs.SelectMany(s => s.Slides).Where(slide => slide.StorageItemId.HasValue)) {
                    // save file

                    // save smaller file

                    // remove storage item

                    // remove storage item id
                }
            }
            // commit changes
            return "OK";
        }
    }
}