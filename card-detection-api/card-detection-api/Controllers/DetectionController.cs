using Microsoft.AspNetCore.Mvc;
using Redoublet.Backend.Models;

namespace Redoublet.Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class DetectionController : ControllerBase
    {
        private readonly ILogger<DetectionController> _logger;

        public DetectionController(ILogger<DetectionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("detect-card")]
        public DetectionResult Post([FromBody] string base64EncodedImage)
        {
            byte[] image = Convert.FromBase64String(base64EncodedImage);
            string imgName = "temp.png";
            
            using (MemoryStream streamBitmap = new MemoryStream(image))
            {
                Image.Load(image).SaveAsPng(imgName);
            }

            var dr = Darknet.ProcessImage(Path.GetFullPath(imgName));
            return dr;
        }
    }
}
