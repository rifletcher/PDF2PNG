using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NReco.PdfRenderer;
using Serilog;

namespace PDF2PNG
{
    public class PdfConverterService
    {
        public string GetImg(string pdfString)
        {
            Log.Information("GetImg Called");

            var pdfFile = Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".pdf");
            var pngFile = Path.ChangeExtension(pdfFile, ".png");
            try
            {
                // decode the incoming image
                var decodedBytes = Convert.FromBase64String(pdfString);

                // save bytes to random temp file
                var logFile = System.IO.File.Create(pdfFile);
                using (BinaryWriter pdfWriter = new BinaryWriter(logFile))
                {
                    pdfWriter.Write(decodedBytes);
                }

                // call nreco library to convert
                var pdfToImg = new NReco.PdfRenderer.PdfToImageConverter();
                pdfToImg.GenerateImage(pdfFile, 1, ImageFormat.Png, pngFile);

                if (!File.Exists(pngFile))
                    throw new Exception("NReco.PdfRenderer.PdfToImageConverter Failed");

                // encode and return
                return Convert.ToBase64String(File.ReadAllBytes(pngFile));

            }
            catch (Exception ex)
            {
                Log.Error("Unexpected error", ex);
                throw ex;
            }
            finally
            {
                // Cleanup
                if (File.Exists(pdfFile))
                    File.Delete(pdfFile);
                if (File.Exists(pngFile))
                    File.Delete(pngFile);
            }
        }

        public Task<string> GetImgAsync(string pdfString)
        {
            Log.Information("GetImgAsync");
            throw new NotImplementedException();
        }

    }
}
