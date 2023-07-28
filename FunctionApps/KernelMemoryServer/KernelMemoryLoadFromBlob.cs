using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Memory.Qdrant;
using Microsoft.SemanticKernel.Memory;
using System.Net.Http;
using SemanticMemoryDocumentImport.blob.MemoryUpload;
using SemanticMemoryDocumentImport;
using System.Threading.Tasks;

namespace KernelMemoryLoad
{

    public class KernelMemoryLoadFromBlob
    {
       
        [FunctionName("KernelMemoryLoadFromBlob")]
        public async Task Run([BlobTrigger("data/{name}", Connection = "BlobConnectionString")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            
            IKernel kernel = CreateKernel;
            ImportResult importResult = await new SemanticMemoryUploadFromBlob(kernel, connectionString: Get_environmentvalue("BlobConnectionString"), containerName: name, collectionName: Get_environmentvalue("QdrantCollection")).ParseDocumentContentToMemoryAsync(myBlob, name);
            if (!importResult.IsSuccessful)
            {
                log.LogError("Memeory upload failed");
            }

        }

        public string Get_environmentvalue(string Name)
        {
            return Environment.GetEnvironmentVariable(Name, EnvironmentVariableTarget.Process);
        }
        public IKernel CreateKernel
        {
            get
            {
                KernelBuilder builder = new KernelBuilder();
                ILogger logger = new Logger<KernelBuilder>(new LoggerFactory());
                builder.WithAzureTextEmbeddingGenerationService(Get_environmentvalue("AzureOpenAiDeployment"),Get_environmentvalue("AzureOpenAiEndpoint"),  Get_environmentvalue("AzureOpenAiKey"));
                builder.WithMemoryStorage(_memoryStore);
                return builder.Build();
            }
        }

        private IMemoryStore _memoryStore
        {
            get
            {
                int port = Int32.Parse(Get_environmentvalue("QdrantPort"));
                int vectorsize = Int32.Parse(Get_environmentvalue("QdrantVectorSize"));
                HttpClient httpClient = new HttpClient(new HttpClientHandler() { CheckCertificateRevocationList = true });
                var endPointBuilder = new UriBuilder(Get_environmentvalue("QdrantHost"));
                endPointBuilder.Port = port;
                return new QdrantMemoryStore(httpClient: httpClient, vectorsize, endPointBuilder.ToString());
            }
        }
    }
}
