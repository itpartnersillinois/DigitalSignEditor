using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.ApiModels;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SignController : ControllerBase {
        private ISecurityHelper securityHelper;
        private ISignRepository signRepository;

        public SignController(ISignRepository signRepository, ISecurityHelper securityHelper) {
            this.signRepository = signRepository;
            this.securityHelper = securityHelper;
        }

        [HttpPost("Add")]
        public async Task<int> AddSign([FromBody] dynamic json) {
            if (!securityHelper.IsAdmin(User)) {
                return 0;
            }
            var college = int.Parse(json.college.ToString());
            var signType = int.Parse(json.college.ToString());

            var sign = new Sign {
                Name = json.name,
                Description = json.description,
                Url = json.url,
                TwitterHandle = "",
                MaximumSize = 0,
                MinimumHeight = 0,
                MinimumWidth = 0,
                RatioHeight = 0,
                RatioWidth = 0,
                IsActive = true,
                College = (CollegeType) college,
                SignType = (SignType) signType,
                LastUpdated = DateTime.Now
            };
            _ = await signRepository.CreateAsync(sign);
            return sign.Id;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<SignGroup> GetAllSigns() {
            var signs = await signRepository.ReadAsync(rep => rep.Signs.Where(s => s.IsActive));
            return new SignGroup(signs.ToList());
        }

        [HttpGet("Get/{id}")]
        public async Task<SignInformation> GetSign(int id) {
            if (!securityHelper.CanAccess(User, id)) {
                return new SignInformation();
            }
            var sign = await signRepository.ReadAsync(rep => rep.Signs.Include(s => s.Slides).FirstOrDefault(s => s.IsActive && s.Id == id));
            return new SignInformation(sign);
        }

        [HttpGet("Get")]
        public async Task<SignGroup> GetSigns() {
            if (!securityHelper.IsLoggedIn(User)) {
                return new SignGroup();
            }
            if (securityHelper.IsAdmin(User)) {
                return await GetAllSigns();
            }
            var signs = await signRepository.ReadAsync(rep => rep.Signs.Include(s => s.SignPermissions).Where(s => s.IsActive && s.SignPermissions.Select(sp => sp.Name).Contains(User.Identity.Name)));
            return new SignGroup(signs.ToList());
        }

        [HttpPost("Update")]
        public async Task<SignInformation> Update([FromBody] dynamic json) {
            var id = (int) json.id;
            if (!securityHelper.CanAccess(User, id)) {
                return new SignInformation();
            }
            var sign = await signRepository.ReadAsync(rep => rep.Signs.FirstOrDefault(s => s.IsActive && s.Id == id));
            sign.Name = json.name;
            sign.Description = json.description;
            string twitter = json.twitter.ToString();
            sign.TwitterHandle = twitter.ConvertTwitterString();
            string maxSize = json.maximumsize.ToString();
            string minHeight = json.minimumheight.ToString();
            string minWidth = json.minimumwidth.ToString();
            string ratio = json.ratio.ToString();
            sign.MaximumSize = maxSize.ConvertStringToInt();
            sign.MinimumHeight = minHeight.ConvertStringToInt();
            sign.MinimumWidth = minWidth.ConvertStringToInt();
            var ratioParsed = ratio.ConvertStringToInts();
            sign.RatioWidth = ratioParsed.Item1;
            sign.RatioHeight = ratioParsed.Item2;

            _ = await signRepository.UpdateAsync(sign);
            return new SignInformation(sign);
        }
    }
}