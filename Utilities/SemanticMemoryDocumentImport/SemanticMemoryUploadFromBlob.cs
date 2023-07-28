
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Text;
using SemanticMemoryDocumentImport.BlobReader;
using SemanticMemoryDocumentImport.TextExtractor;
namespace SemanticMemoryDocumentImport.blob.MemoryUpload
{
    public class SemanticMemoryUploadFromBlob
    {
        private IKernel kernel;

        private string connectionString;

        private string containerName;

        private string collectionName;
        /// <summary>
        /// Parse the given Filename and parse document to vecator memory.
        /// </summary>
        /// <param name="kernel">Kernel Object with Memory Storage and Embedding </param>
        /// <param name="connectionString">BlobConnectionString</param>
        /// <param name="collectionName">Collection Name in Vector index</param>
        /// <param name="containerName">Name of the blob container to be processed</param>
        public SemanticMemoryUploadFromBlob(IKernel kernel, string connectionString , string containerName,string collectionName)
        {
            this.kernel = kernel;
            this.connectionString = connectionString;
            this.containerName = containerName;
            this.collectionName = collectionName;
        }
        /// <summary>
        /// Parse the given Filename and parse document to vecator memory.
        /// </summary>
        /// <param name="blobName">Filename</param>
        /// <returns name=ImportResults>Final Result in Importresults</returns>
        public async Task<ImportResult> ParseDocumentContentToMemoryAsync(string blobName )
        {
            ImportResult importResult = new ImportResult();
            // Get the document content from the blob container
            SemanticMemoryBlobReader reader = new SemanticMemoryBlobReader(connectionString);
            foreach (var container in reader.GetallBlobContainers().Where<string>(x => x.Equals(containerName)))
            {

                foreach (var blob in reader.GetallBlobs(container).Where<string>(x=>x.Equals(blobName)))
                {
                    var filetextcontents = await reader.ReadBlobContent(container, blob, collectionName);
                    List<string> chunk_text = this.chunk_text(filetextcontents);
                    importResult = await this.save_to_memory(chunk_text, blob);
                    return importResult;
                }

            }
            return importResult;
        }

        /// <summary>
        ///Read all blobs from container and save to Vector Memory
        /// </summary>
        /// <returns name=ImportResults>Final Result in Importresults</returns>
        public async Task<ImportResult> ParseDocumentContentToMemoryAsync( )
        {
            ImportResult importResult = new ImportResult();
            // Get the document content from the blob container
            SemanticMemoryBlobReader reader = new SemanticMemoryBlobReader(connectionString);
            foreach (var container in reader.GetallBlobContainers().Where<string>(x=>x.Equals(containerName)))
            {
               
                foreach (var blob in reader.GetallBlobs(container))
                {
                    var filetextcontents = await reader.ReadBlobContent(container, blob, collectionName);
                    List<string> chunk_text = this.chunk_text(filetextcontents);
                    importResult = await this.save_to_memory(chunk_text,blob);
                    return importResult;
                }
               
            }
            return importResult;
        }

        /// <summary>
        ///use filestream and save to Vector Memory
        /// </summary>
        /// <param name="blobContent">Blob File content</param>
        /// <param name="blobType">FileType</param>
        /// <param name="blobName">The Filename from blob</param>
        /// <returns name=ImportResults>Final Result in Importresults</returns>
        public async Task<ImportResult> ParseDocumentContentToMemoryAsync(Stream blobContent,  String blobName )
        {
            ImportResult importResult = new ImportResult();
            string extension = Path.GetExtension(blobName);
            string blobType = extension.Substring(extension.LastIndexOf('.') + 1);
            ISemanticMemoryFileExtractor factory = SemanticMemoryTextExtractorFactory.GetExtractor(blobType);
            var fieldcontents = await factory.ExtractFileAsync(blobContent);
            var chunked_text = this.chunk_text(fieldcontents);
            importResult = await this.save_to_memory(chunked_text, blobName);

            return  importResult;
        }
        /// <summary>
        ///Chunk text with given positions
        /// </summary>
        /// <param name="text">Whole Text that need to be chunked</param>
        /// <returns name=paragraphs>Return the text in paragraphs chunks</returns>
        private List<string> chunk_text(string text)
        {
            var lines = TextChunker.SplitPlainTextLines(text, 30);
            List<string> paragraphs = TextChunker.SplitPlainTextParagraphs(lines, 100);
            return paragraphs;
        }
        /// <summary>
        ///Store chunked text to Vector
        /// </summary>
        /// <param name="chunkedText">Chunked paragraphs</param>
        /// <param name="blobName">Blob Filename</param>
        /// <returns name=paragraphs>Return the text in paragraphs chunks</returns>
        private async  Task<ImportResult> save_to_memory(List<string> chunkedText,   string blobName)
        {
            var importResult = new ImportResult(collectionName);
            if (chunkedText != null)
            {
                for (int i = 0; i < chunkedText.Count; i++)
                {
                    var pragraph = chunkedText[i];
                    var paragraphId = Guid.NewGuid().ToString();
                    await kernel.Memory.SaveInformationAsync(
                        collection: collectionName,
                        text: pragraph,
                    id: paragraphId,
                        description: $"Document: {blobName}");
                    importResult.AddKey(paragraphId);
                }
            }
            return  importResult;
        }

    }
}
