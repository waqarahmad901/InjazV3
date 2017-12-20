using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TranningWebApp.Models;

namespace TmsWebApp.HelpingUtilities
{
    public class PdfGenerator
    {
        public static string GenerateOnflyPdf(string filePath,  List<PdfCoordinatesModel> pdfCoordinates,string fontFilePath)
        {
            string oldFile = filePath;
            string newFile = Path.Combine(Path.GetDirectoryName(filePath),DateTime.Now.ToString("ddMMyyyyhhmmsstt")+".pdf");

            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            fontFilePath = @"c:\windows\fonts\tahoma.ttf";
            BaseFont bf = BaseFont.CreateFont(fontFilePath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font font = new Font(bf, 20);
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    PdfContentByte canvas = stamper.GetOverContent(1);
                
                    foreach (var item in pdfCoordinates)
                    { 
                        ColumnText.ShowTextAligned(canvas, item.Alignment, new Phrase(item.Text,font), item.X, item.Y, 0, PdfWriter.RUN_DIRECTION_RTL, 1);
                    }
                }
                bytes = ms.ToArray();
            }
            File.WriteAllBytes(newFile, bytes);
             
            return newFile;
        }
    }
}
