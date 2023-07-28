using Azure.Storage.Blobs.Models;

namespace SemanticMemoryDocumentImport.TextExtractor;

public interface ISemanticMemoryFileExtractor
 {
    Task<ImportResult> ExtractFileAsync(string filePath);

    Task<string> ExtractFileAsync(BlobDownloadResult content );

    Task<string> ExtractFileAsync(Stream content);

    Task<string> ExtractFileasChunkAsync(BlobDownloadResult content);
}