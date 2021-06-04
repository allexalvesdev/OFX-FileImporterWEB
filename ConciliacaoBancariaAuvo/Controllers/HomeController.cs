using ConciliacaoBancariaAuvo.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ConciliacaoBancariaAuvo.Data;
using X.PagedList;


namespace ConciliacaoBancariaAuvo.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var search = Request.Query["Search"].ToString();

            var lista = _context.Extratos
                .OrderBy(c => c.Tipo)
                .Where(c => c.Tipo.Contains(search));

            PagedList<Extrato> model = new PagedList<Extrato>(lista, page, pageSize);
            return View("Index", model);

        }

        [HttpPost]
        public IActionResult Create(IFormFile arquivo, Extrato extrato)
        {

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UploadArquivo(IFormFile arquivo, string ofxPrefixo)
        {

            string fileContents;
            using (var stream = arquivo.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    fileContents = await reader.ReadToEndAsync();
                }
            }

            List<string> listaTags = new List<string>();
            var extrato = new Extrato();
            List<Extrato> extratos = new List<Extrato>();

            while (fileContents.IndexOf("<STMTTRN>") > -1)
            {
                var indexInicial = fileContents.IndexOf("<STMTTRN>");
                var indexFinal = fileContents.IndexOf("</STMTTRN>") + 10;

                var intervaloIndex = fileContents.Substring(indexInicial, (indexFinal - indexInicial));

                listaTags.Add(intervaloIndex);

                fileContents = fileContents.Substring(indexFinal);

            }

            foreach (var list in listaTags)
            {
                #region teste
                //var posTipo = fileContents.IndexOf("<TRNTYPE>");
                //listaTags[0] = intervaloIndex.Substring(19, 5);
                //extrato.Tipo = listaTags[0];
                //listaTags[0] = intervaloIndex.Substring(35, 8);
                ////extrato.DataLancamento = Convert.ToDateTime(listaTags[0]);
                //extrato.DataLancamento = listaTags[0];
                //listaTags[0] = intervaloIndex.Substring(67, 6);
                //extrato.Valor = Convert.ToDecimal(listaTags[0]);
                //listaTags[0] = intervaloIndex.Substring(81, 25);
                //extrato.Descricao = listaTags[0];

                //<STMTTRN>
                //<TRNTYPE> DEBIT
                //<DTPOSTED> 20140203100000[-03:EST]
                //<TRNAMT> -140.00
                //<MEMO> CXE     001958 SAQUE
                //</STMTTRN >
                #endregion

                extrato.Id = extrato.Id;

                var descricaoInicial = list.IndexOf("<MEMO>");

                //var indexfinal = listaTags[i].IndexOf("</STMTTRN>") - 82;
                var valorFinal = list.IndexOf("<MEMO>") - 68;

                extrato.Tipo = list.ToString().Substring(19, 5);
                extrato.DataLancamento = list.ToString().Substring(35, 8);
                extrato.Valor = list.ToString().Substring(67, valorFinal);
                extrato.Descricao = list.ToString().Substring(descricaoInicial, 25).Replace("<MEMO>", "");

                extratos.Add(new Extrato(extrato.Tipo, extrato.DataLancamento, extrato.Valor, extrato.Descricao));

            }

            _context.Extratos.AddRange(extratos);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var extrato = _context.Extratos.Find(id);

            return View(extrato);
        }

    }
}
