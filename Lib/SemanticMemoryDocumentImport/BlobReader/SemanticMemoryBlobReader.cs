using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SemanticMemoryDocumentImport.TextExtractor;
namespace SemanticMemoryDocumentImport.BlobReader
{
    public class SemanticMemoryBlobReader
    {

        private BlobServiceClient blobServiceClient;

        public SemanticMemoryBlobReader(String ConnectionString)
        {
            try
            {
                if (String.IsNullOrEmpty(ConnectionString))
                {
                    throw new ArgumentNullException("ConnectionString");
                }
                blobServiceClient = new BlobServiceClient(ConnectionString);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
   
      private BlobContainerClient GetBlobContainerClient(String ContainerName)
      {
          try
          {
              if (String.IsNullOrEmpty(ContainerName))
              {
                  throw new ArgumentNullException("ContainerName");
              }
              return blobServiceClient.GetBlobContainerClient(ContainerName);
          }
          catch (ArgumentNullException e)
          {
              Console.WriteLine(e.Message);
              throw;
          }
          catch (ArgumentException e)
          {
              Console.WriteLine(e.Message);
              throw;
          }
          catch (Exception e)
          {
              Console.WriteLine(e.Message);
              throw;
          }
      }

      public List<String> GetallBlobContainers(string searchPattern = "*"){
            try{
                var  containers = blobServiceClient.GetBlobContainers();
                var containerNames = new List<String>();
                
                foreach (var container in containers)
                {
                    if (container.Name.Contains(searchPattern) || searchPattern == "*")
                    {
                        containerNames.Add(container.Name);
                    }
                }
                return containerNames;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
      }
    
    public List<String> GetallBlobs(String ContainerName ){
            try{
                BlobContainerClient  containerClient = GetBlobContainerClient(ContainerName);
                Azure.Pageable<BlobItem> blobs = containerClient.GetBlobs();
                
                var blobNames = new List<String>();
                foreach (BlobItem blob in blobs)
                {   
                
                    blobNames.Add(blob.Name);
                 
                }
                return blobNames;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
      }
      public   Task<string> ReadBlobContent(String ContainerName, String BlobName, string collectionName){
             var importResult = new ImportResult(collectionName);
            try{
                BlobContainerClient  containerClient = GetBlobContainerClient(ContainerName);
                BlobClient  blobClient = containerClient.GetBlobClient(BlobName);            
                BlobDownloadResult result = blobClient.DownloadContent();
                var data = result.Content.ToMemory();
                string extension = Path.GetExtension(BlobName);
                string ext = extension.Substring(extension.LastIndexOf('.') + 1);

                var fieldcontent = converttotextAsync(result, ext);
                return Task<String>.FromResult(fieldcontent.ToString());
             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
      }
        private async Task<string> converttotextAsync(BlobDownloadResult result, string type)
        {
            ISemanticMemoryFileExtractor extractor = SemanticMemoryTextExtractorFactory.GetExtractor(type);
            string fileContent = await extractor.ExtractFileAsync(result);
            return fileContent;
        }

        public async Task<string> converttotextAsync(Stream result, string type)
        {
            ISemanticMemoryFileExtractor extractor = SemanticMemoryTextExtractorFactory.GetExtractor(type);
            string fileContent = await extractor.ExtractFileAsync(result);
            return fileContent;
        }

    }
}


