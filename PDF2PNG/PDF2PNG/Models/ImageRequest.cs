using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDF2PNG.Models
{
    public class ImageRequest
    {
        public string PdfString { get; set; }
    }

    public class ImageResponse
    {
        public string PngString { get; set; }
    }
}
