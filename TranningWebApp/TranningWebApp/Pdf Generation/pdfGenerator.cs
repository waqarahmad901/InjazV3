using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace TranningWebApp.Pdf_Generation
{
    public class pdfGenerator
    {
        public string GeneratePdf(string pdfData, string title = null, List<string> parameters = null, List<string> results = null,string culture ="en",string fontFilePath = "")
        {
            //string newFile = System.Web.HttpContext.Current.Server.MapPath("Content/pdf");
            string newFile = Path.Combine(HttpRuntime.AppDomainAppPath, "Content\\pdf");
            if (!Directory.Exists(newFile))
            {
                Directory.CreateDirectory(newFile);
            }
            newFile = System.IO.Path.Combine(newFile, Guid.NewGuid().ToString() + "-Certificate.pdf");

            byte[] GraphImage = Convert.FromBase64String(pdfData);
            Image Image = null;

            if (!string.IsNullOrEmpty(pdfData))
                Image = Image.GetInstance(GraphImage);


            Document doc = new Document();


            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            BaseFont bf = BaseFont.CreateFont(fontFilePath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font font = new Font(bf, 12);

            PdfPTable table = new PdfPTable(1); // one column table
            if(culture == "ar")
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

            PdfPCell cell;
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            if (title != null)
            {
            
            Font fontheader = new Font(bf, 16, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, BaseColor.DARK_GRAY);

                cell = new PdfPCell(new Phrase(title, fontheader));
                cell.FixedHeight = 55f;
                cell.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                cell.Border = Rectangle.NO_BORDER;
                table.AddCell(cell);
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    cell = new PdfPCell(new Phrase(parameter, font));
                    cell.FixedHeight = 25f;
                    cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                }
            }

            if (Image != null)
            {
                Image.ScaleAbsolute(500f, 300f);
                cell = new PdfPCell(Image);
                cell.Border = Rectangle.NO_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                table.AddCell(cell);
            }
            doc.Add(table);

            if (results != null)
            {
                var colunmns = results[0].Split(':').Count();
                table = new PdfPTable(colunmns); // two columns table
                if (culture == "ar")
                    table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                table.WidthPercentage = 100;

                foreach (var result in results)
                {
                    var columns = result.Split(':');
                    cell = new PdfPCell(new Phrase(columns[0], font));
                    cell.FixedHeight = 25f;
                    // cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(columns[1], font));
                    cell.FixedHeight = 25f;
                    // cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    if (columns.Length > 2)
                    {
                        cell = new PdfPCell(new Phrase(columns[2], font));
                        cell.FixedHeight = 25f;
                        // cell.Border = Rectangle.NO_BORDER;
                        table.AddCell(cell);
                    }
                    if (columns.Length > 3)
                    {
                        cell = new PdfPCell(new Phrase(columns[3], font));
                        cell.FixedHeight = 25f;
                        // cell.Border = Rectangle.NO_BORDER;
                        table.AddCell(cell);
                    }
                }
            }
            doc.Add(table);



            doc.Close();
            writer.Close();
            fs.Close();


            return newFile;
        }

    }
}