using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    public class SignItemController : ControllerBase {
        private readonly ISecurityHelper securityHelper;
        private readonly ISignRepository signRepository;

        public SignItemController(ISignRepository signRepository, ISecurityHelper securityHelper) {
            this.signRepository = signRepository;
            this.securityHelper = securityHelper;
        }

        [HttpPost("Add")]
        public async Task<int> AddSignItem([FromBody] dynamic json) {
            var jsonObject = JObject.Parse(json.ToString());
            int signId = int.Parse(jsonObject.signId.ToString());
            if (!securityHelper.CanAccess(User, signId)) {
                return default;
            }
            var signItem = await signRepository.ReadAsync(rep => rep.Slides.FirstOrDefault(s => s.Id == signId));
            ChangedHelper.SignChanged(signRepository, null, null, signItem.Name);
            ChangedHelper.UpdateSignLastUpdated(signRepository, signId);
            var totalItems = await signRepository.ReadAsync(rep => rep.Slides.Count(s => s.SignId == signId));
            return await signRepository.CreateAsync(new Slide {
                IsActive = true,
                LastUpdated = DateTime.Now,
                SignId = signId,
                Name = jsonObject.name,
                Data = jsonObject.data,
                Option = jsonObject.option,
                Order = totalItems + 1,
                Url = jsonObject.option == SlideType.Video ? jsonObject.data.ToString().Split(";")[0] : "",
            });
        }

        [HttpPost("Move/{id}/{direction}")]
        public async Task<int> MoveSignItem(int id, string direction) {
            var signItem = await signRepository.ReadAsync(rep => rep.Slides.FirstOrDefault(s => s.Id == id));
            var signId = await signRepository.ReadAsync(rep => rep.Slides.FirstOrDefault(s => s.Id == id)?.SignId);
            if (signId == null || !securityHelper.CanAccess(User, signId.Value)) {
                return default;
            }
            ChangedHelper.SignChanged(signRepository, null, null, signItem.Name);
            ChangedHelper.UpdateSignLastUpdated(signRepository, signId.Value);
            // need to reset order to ensure everything is in numerical order with no gaps
            int tempOrder = default;
            Slide sign = default;
            var allSigns = await signRepository.ReadAsync(rep => rep.Slides.Where(s => s.SignId == signId.Value));
            var allSignsList = allSigns.OrderBy(s => s.Order).ThenBy(s => s.Name).ToList();
            for (int i = 0; i < allSignsList.Count(); i++) {
                allSignsList[i].Order = i + 1;
                if (allSignsList[i].Id == id) {
                    tempOrder = allSignsList[i].Order;
                    sign = allSignsList[i];
                }
            }
            var signSwap = direction.Equals("up", StringComparison.OrdinalIgnoreCase) ?
                allSignsList.OrderByDescending(s => s.Order).FirstOrDefault(s => s.Order < tempOrder) :
                allSignsList.OrderBy(s => s.Order).FirstOrDefault(s => s.Order > tempOrder);
            if (signSwap != null) {
                sign.Order = signSwap.Order;
                signSwap.Order = tempOrder;
            }
            foreach (var signInList in allSignsList) {
                _ = await signRepository.UpdateAsync(signInList);
            }

            return 1;
        }

        [HttpPost("Remove/{id}")]
        public async Task<int> RemoveSignItem(int id) {
            var sign = await signRepository.ReadAsync(rep => rep.Slides.FirstOrDefault(s => s.Id == id));
            if (sign == null || !securityHelper.CanAccess(User, sign.SignId)) {
                return default;
            }
            ChangedHelper.SignChanged(signRepository, null, null, sign.Name);
            ChangedHelper.UpdateSignLastUpdated(signRepository, sign.SignId);
            var order = sign.Order;
            var signId = sign.SignId;
            await signRepository.DeleteAsync(sign);
            var signsAfter = await signRepository.ReadAsync(rep => rep.Slides.Where(s => s.SignId == signId && s.Order > order));
            foreach (var signAfter in signsAfter) {
                signAfter.Order = signAfter.Order - 1;
                await signRepository.UpdateAsync(signAfter);
            }
            return 1;
        }

        [HttpPost("Update")]
        public async Task<int> UpdateSignItem([FromBody] dynamic json) {
            var jsonObject = JObject.Parse(json.ToString());
            int id = int.Parse(jsonObject.id.ToString());
            var sign = await signRepository.ReadAsync(rep => rep.Slides.FirstOrDefault(s => s.Id == id));
            if (sign == null || !securityHelper.CanAccess(User, sign.SignId)) {
                return default;
            }
            var startDate = TextHelper.ConvertDate(jsonObject.startDate.ToString());
            var endDate = TextHelper.ConvertDate(jsonObject.endDate.ToString());
            ChangedHelper.SignChanged(signRepository, startDate, endDate, sign.Name);
            ChangedHelper.UpdateSignLastUpdated(signRepository, sign.SignId);
            sign.LastUpdated = DateTime.Now;
            sign.Name = jsonObject.name;
            sign.Description = jsonObject.description;
            sign.IsActive = jsonObject.isActive;
            sign.EndDate = endDate;
            sign.StartDate = startDate;
            return await signRepository.UpdateAsync(sign);
        }
    }
}