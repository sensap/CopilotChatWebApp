using Azure.Storage.Blobs.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
namespace SemanticMemoryDocumentImport.TextExtractor
{
    internal class WordTextExtractor : ISemanticMemoryFileExtractor
    
    {
Task<string> ISemanticMemoryFileExtractor.ExtractFileasChunkAsync(BlobDownloadResult content)
        {
            throw new NotImplementedException();
        }

        Task<ImportResult> ISemanticMemoryFileExtractor.ExtractFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        Task<string> ISemanticMemoryFileExtractor.ExtractFileAsync(BlobDownloadResult content)
        {
            StringBuilder sb = new StringBuilder();
            var stream = content.Content.ToStream();
            //string text = stream.ReadToEnd();
           // foreach ( string text in Encoding.Default.GetString(content.Content.ToArray()).Split('\n'))
           //{
           //    sb.Append(text);
           // }
           WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(stream, false);
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;
         
            string innerText = body.InnerText;           
            return Task<string>.FromResult(innerText);
           
            
        }

        Task<string> ISemanticMemoryFileExtractor.ExtractFileAsync(Stream content)
        {
           
            WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(content, false);
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            string innerText = body.InnerText;
            return Task<string>.FromResult(innerText);


        }
    }
}
