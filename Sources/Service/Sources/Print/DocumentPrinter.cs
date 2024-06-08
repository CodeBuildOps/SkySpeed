using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Diagnostics;

namespace SkySpeedService.Print
{
    class DocumentPrinter
    {
        public bool PrintWordDocument(string pnrFilePath, string htmlContent)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(pnrFilePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();

                    mainPart.Document = new Document();
                    Body body = new Body();
                    mainPart.Document.Append(body);

                    HtmlConverter converter = new HtmlConverter(mainPart);
                    var paragraphs = converter.Parse(htmlContent);

                    foreach (var paragraph in paragraphs)
                    {
                        body.Append(paragraph);
                    }

                    mainPart.Document.Save();
                    Process.Start(pnrFilePath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}