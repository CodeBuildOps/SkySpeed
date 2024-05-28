using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using DocumentFormat.OpenXml;
using SkySpeedService.PNR;

namespace SkySpeedService.Print
{
    class DocumentPrinter : HTMLBody
    {
        private const string _docxFile = "PNR.docx";

        public bool PrintDocument()
        {
            try
            {
                GenerateWordDocument(Path.Combine(Environment.CurrentDirectory, _docxFile));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GenerateWordDocument(string filePath)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = new Body();
                mainPart.Document.Append(body);

                HtmlConverter converter = new HtmlConverter(mainPart);
                var paragraphs = converter.Parse(_htmlBody);

                foreach (var paragraph in paragraphs)
                {
                    body.Append(paragraph);
                }

                mainPart.Document.Save();
            }
        }
    }
}