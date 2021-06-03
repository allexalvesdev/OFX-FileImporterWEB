using Microsoft.AspNetCore.Http;

namespace ConciliacaoBancariaAuvo.Entities
{
    public class Extrato
    {

        public IFormFile OfxUpload { get; set; }

        public string Ofx { get; set; }
    }
}
