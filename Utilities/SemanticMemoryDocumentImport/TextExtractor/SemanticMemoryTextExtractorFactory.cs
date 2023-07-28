using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticMemoryDocumentImport.TextExtractor
{
    public class SemanticMemoryTextExtractorFactory  {
        public static ISemanticMemoryFileExtractor GetExtractor(string type)  {
            switch (type)  {
                case "pdf":
                    return new PdfTextExtractor();
                case "txt":
                  return new DefaultTextExtractor();
                case "ppt" or "pptx":
                    return new DefaultTextExtractor();
                case "doc" or "docx":
                    return new WordTextExtractor();
                default:
                    return new DefaultTextExtractor();
            }
        }
    }
}
 
