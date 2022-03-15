using System.IO;
using System.Linq;
using DigitalSignEditor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DigitalSignEditor.Controllers {

    [Route("[controller]")]
    public class ImageController : Controller {
        private readonly ISignRepository signRepository;

        public ImageController(ISignRepository signRepository) {
            this.signRepository = signRepository;
        }

        [HttpGet("{id}")]
        public void Index(int id) {
            var storage = signRepository.Read(rep => rep.StorageItems.FirstOrDefault(si => si.Id == id));
            if (storage == null) {
                Response.StatusCode = 404;
            }
            Response.StatusCode = 200;
            string contentType;
            if (!new FileExtensionContentTypeProvider().TryGetContentType(storage.Name, out contentType)) {
                contentType = "application/unknown";
            }
            Response.ContentType = contentType;
            var stream = new MemoryStream(storage.Data);
            stream.CopyToAsync(Response.Body);
        }
    }
}