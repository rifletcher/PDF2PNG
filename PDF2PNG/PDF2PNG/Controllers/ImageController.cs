using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDF2PNG.Models;

namespace PDF2PNG.Controllers
{
    public class ImageController : Controller
    {
        [HttpPost]
        [Route("pdfconverter")]
        public IActionResult Post([FromBody]ImageRequest imageRequest)
        {
            if (string.IsNullOrEmpty(imageRequest?.PdfString))
                return NotFound("No PDF supplied");
            try
            {
                var pdfService = new PdfConverterService();
                var result = new ImageResponse() { PngString = pdfService.GetImg(imageRequest.PdfString) };
                return new ObjectResult(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
    }
}