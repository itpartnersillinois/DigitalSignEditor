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
        private SecurityHelper securityHelper;
        private ISignRepository signRepository;

        public SignItemController(ISignRepository signRepository, SecurityHelper securityHelper) {
            this.signRepository = signRepository;
            this.securityHelper = securityHelper;
        }

        [HttpPost("Add")]
        public async Task<int> AddSignItem([FromBody] dynamic json) {
            var jsonObject = (dynamic) JObject.Parse(json.ToString());
            int signId = int.Parse(jsonObject.signId.ToString());
            if (!securityHelper.CanAccess(User, signId)) {
                return default;
            }
            var totalItems = await signRepository.ReadAsync(rep => rep.SignItems.Count(s => s.SignId == signId));
            return await signRepository.CreateAsync(new SignItem {
                IsActive = true,
                LastUpdated = DateTime.Now,
                Name = jsonObject.name,
                Data = jsonObject.data,
                Description = jsonObject.description,
                EndDate = DateTime.Parse(jsonObject.endDate.ToString()),
                StartDate = DateTime.Parse(jsonObject.startDate.ToString()),
                Option = jsonObject.option,
                Order = totalItems + 1
            });
        }

        [HttpPost("Move/{id}/{direction}")]
        public async Task<int> MoveSignItem(int id, string direction) {
            var sign = await signRepository.ReadAsync(rep => rep.SignItems.FirstOrDefault(s => s.Id == id));
            if (sign == null || !securityHelper.CanAccess(User, sign.SignId)) {
                return default;
            }
            var tempOrder = sign.Order;
            var signSwap = direction == "up" ?
                await signRepository.ReadAsync(rep => rep.SignItems.FirstOrDefault(s => s.SignId == sign.SignId && s.Order < tempOrder)) :
                await signRepository.ReadAsync(rep => rep.SignItems.FirstOrDefault(s => s.SignId == sign.SignId && s.Order > tempOrder));
            sign.Order = signSwap.Order;
            signSwap.Order = tempOrder;
            _ = await signRepository.UpdateAsync(sign);
            _ = await signRepository.UpdateAsync(signSwap);
            return 1;
        }

        [HttpPost("Remove/{id}")]
        public async Task<int> RemoveSignItem(int id) {
            var sign = await signRepository.ReadAsync(rep => rep.SignItems.FirstOrDefault(s => s.Id == id));
            if (sign == null || !securityHelper.CanAccess(User, sign.SignId)) {
                return default;
            }
            return await signRepository.DeleteAsync(sign);
        }

        [HttpPost("Update")]
        public async Task<int> UpdateSignItem([FromBody] dynamic json) {
            var jsonObject = (dynamic) JObject.Parse(json.ToString());
            int id = int.Parse(jsonObject.id.ToString());
            var sign = await signRepository.ReadAsync(rep => rep.SignItems.FirstOrDefault(s => s.Id == id));
            if (sign == null || !securityHelper.CanAccess(User, sign.SignId)) {
                return default;
            }
            sign.LastUpdated = DateTime.Now;
            sign.Name = jsonObject.name;
            sign.Data = jsonObject.data;
            sign.Description = jsonObject.description;
            sign.EndDate = DateTime.Parse(jsonObject.endDate.ToString());
            sign.StartDate = DateTime.Parse(jsonObject.startDate.ToString());
            sign.Option = jsonObject.option;
            return await signRepository.UpdateAsync(sign);
        }
    }
}