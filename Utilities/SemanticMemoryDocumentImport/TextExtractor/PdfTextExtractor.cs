using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

using Azure.Storage.Blobs.Models;
namespace SemanticMemoryDocumentImport.TextExtractor
{

    public class PdfTextExtractor : ISemanticMemoryFileExtractor
    {
        public Task<ImportResult> ExtractFileAsync(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ExtractFileAsync(BlobDownloadResult content)
        {
            if (content != null)
            {
                var pdf = PdfDocument.Open(content.Content.ToStream());
                var pages = pdf.GetPages();
                var fileContent = string.Empty;
                foreach (var page in pages)
                {
                    var text = ContentOrderTextExtractor.GetText(page);
                    fileContent += text;
                }
                return Task.FromResult<string>(fileContent);
            }
            else
            {
                return Task.FromResult<string>(null);
            }


        }

        public Task<string> ExtractFileAsync(Stream content)
        {
            if (content != null)
            {
                var pdf = PdfDocument.Open(content);
                var pages = pdf.GetPages();
                var fileContent = string.Empty;
                foreach (var page in pages)
                {
                    var text = ContentOrderTextExtractor.GetText(page);
                    fileContent += text;
                }
                return Task.FromResult<string>(fileContent);
            }
            else
            {
                return Task.FromResult<string>(null);
            }


        }

        public Task<string> ExtractFileasChunkAsync(BlobDownloadResult content)
        {
            throw new System.NotImplementedException();
        }
    }
}