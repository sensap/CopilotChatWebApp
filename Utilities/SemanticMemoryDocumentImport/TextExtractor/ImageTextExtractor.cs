using Azure.Storage.Blobs.Models;
using Tesseract;
using SemanticKernel.Service.Services;
namespace SemanticMemoryDocumentImport.TextExtractor
{
    public class ImageTextExtractor : ISemanticMemoryFileExtractor
    
    {
        public Task<string> ExtractFileasChunkAsync(BlobDownloadResult content)
        {
            throw new NotImplementedException();
        }

        public Task<ImportResult> ExtractFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ExtractFileAsync(BlobDownloadResult content)
        {
            
            var a = new MemoryStream();
            content.Content.ToStream().CopyTo(a);
            var filebytes = a.ToArray();
            
            TesseractEngine tesseractEngine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
            using var img = Pix.LoadFromMemory(filebytes);
            using var page = tesseractEngine.Process(img);
            return Task<string>.FromResult(page.GetText());
        }

        public Task<string> ExtractFileAsync(Stream content)
        {
            //using var streamReader = new StreamReader(content);
            //return  streamReader.ReadToEndAsync();

            var  a = new MemoryStream();
            content.CopyTo(a);
            var filebytes = a.ToArray();
            TesseractEngine tesseractEngine = new TesseractEngine("./data", "eng", EngineMode.Default);
            using var img = Pix.LoadFromMemory(filebytes);
            using  var page = tesseractEngine.Process(img);
            return Task<string>.FromResult(page.GetText());
        }
    }
}
