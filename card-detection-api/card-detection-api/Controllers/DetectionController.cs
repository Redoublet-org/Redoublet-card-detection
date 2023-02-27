using Microsoft.AspNetCore.Mvc;
using object_detection_backend;
using SixLabors.ImageSharp;

namespace card_detection_api.Controllers
{
    [ApiController]
    [Route("detect-cards")]
    public class DetectionController : ControllerBase
    {
        private readonly ILogger<DetectionController> _logger;

        public DetectionController(ILogger<DetectionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public DetectionResult Post([FromBody] string jemoeder)
        {
            byte[] image = Convert.FromBase64String(jemoeder);
            string imgname = "temp.png";
            
            using (MemoryStream streamBitmap = new MemoryStream(image))
            {
                Image.Load(image).SaveAsPng(imgname);
            }

            var dr = Darknet.ProcessImage(Path.GetFullPath(imgname));
            return dr;
        }
    }
}