using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Redoublet.Backend.Models;
using SixLabors.ImageSharp;

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