using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using System.Diagnostics;

namespace SkySpeedService.Print
{
    internal class DocumentPrinter
    {
        public bool PrintWordDocument(string pnrFilePath, string htmlContent)
        {
            bool isDocPrinted = false;
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(pnrFilePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = new Body();
                mainPart.Document.Append(body);

                HtmlConverter converter = new HtmlConverter(mainPart);
                var paragraphs = converter.Parse(htmlContent);

                foreach (OpenXmlCompositeElement paragraph in paragraphs)
                {
                    body.Append(paragraph);
                }

                mainPart.Document.Save();
                Process.Start(pnrFilePath);
                isDocPrinted = true;
            }
            return isDocPrinted;
        }
    }
}