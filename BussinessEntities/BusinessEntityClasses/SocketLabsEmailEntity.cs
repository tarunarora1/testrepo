using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessEntities.BusinessEntityClasses
{
   public class SocketLabsEmailEntity
    {
        public string SocketLabsApiKey { get; set; } 
        public string SocketLabsServerId { get; set; }
        public string SocketLabsUrl { get; set; }
        public string EmailDefaultFrom { get; set; }
        public string SocketAPITemplateKey { get; set; }
         public List<Tuple<string, string>> DocsFile { get; set; }
        public string Toemail  { get; set; }
        public string ccemail { get; set; }
        public string Message { get; set; }
        public string WONumber { get; set; }
        public Guid ClientId { get; set; }
        public string Attachments { get; set; }
    }
}
