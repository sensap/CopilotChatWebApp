using Azure.Storage.Blobs.Models;

namespace SemanticMemoryDocumentImport.TextExtractor
{
    public class DefaultTextExtractor : ISemanticMemoryFileExtractor
    
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
            return Task<string>.FromResult(content.Content.ToString());
        }

        public Task<string> ExtractFileAsync(Stream content)
        {
            using var streamReader = new StreamReader(content);
            return  streamReader.ReadToEndAsync();
        }
    }
}
