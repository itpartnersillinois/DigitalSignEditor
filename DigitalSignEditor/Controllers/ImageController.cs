using System.IO;
using System.Linq;
using DigitalSignEditor.Data;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Controllers {

    [Route("[controller]")]
    public class ImageController : Controller {
        private readonly ISignRepository signRepository;

        public ImageController(ISignRepository signRepository) {
            this.signRepository = signRepository;
        }

        [HttpGet("donor/{id}")]
        public void Donor(int id) {
            var storage = signRepository.Read(rep => rep.Donors.FirstOrDefault(d => d.Id == id));
            if (storage == null) {
                Response.StatusCode = 404;
            }
            Response.StatusCode = 200;
            var stream = new MemoryStream(storage.Image);
            stream.CopyToAsync(Response.Body);
        }

        [HttpGet("{id}")]
        public void Index(int id) {
            var storage = signRepository.Read(rep => rep.StorageItems.FirstOrDefault(si => si.Id == id));
            if (storage == null || storage.Data == null) {
                Response.StatusCode = 404;
            }
            Response.StatusCode = 200;
            Response.ContentType = "image/unknown";
            var stream = new MemoryStream(storage.Data);
            stream.CopyToAsync(Response.Body);
        }
    }
}