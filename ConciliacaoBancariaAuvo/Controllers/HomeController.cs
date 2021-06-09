using ConciliacaoBancariaAuvo.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using ConciliacaoBancariaAuvo.Data;
using System;
using ConciliacaoBancariaAuvo.Services;

namespace ConciliacaoBancariaAuvo.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var search = Request.Query["Search"].ToString();

            var lista = _context.Extratos
                .OrderBy(c => c.DataLancamento)
                .Where(c => c.Descricao.Contains(search) ||
                c.Observacao.Contains(search));


            return View("Index", lista);

        }

        [HttpPost]
        public IActionResult Create(IFormFile arquivo, Extrato extrato)
        {

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UploadArquivo(IFormFile[] arquivos)
        {

            var extratoService = new ExtratoService(_context);

            foreach (var arquivo in arquivos)
            {
                string fileContents;

                using (var stream = arquivo.OpenReadStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        fileContents = await reader.ReadToEndAsync();
                    }
                }

                var lancamentosOfx = extratoService.ExtrairLancamentosOfx(fileContents);

                extratoService.MapearSalvarExtratos(lancamentosOfx);

                TempData["Cont"] = _context.Extratos.Count();
                TempData["Success"] = "Arquivo(s) importado com Sucesso!";
            }
       
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult DetalhesExtrato(Guid id)
        {
            var extrato = _context.Extratos.Find(id);

            var objetoRetorno = new
            {
                Id = extrato.Id,
                Tipo = extrato.Tipo == "CREDIT" ? "CRÉDITO" : "DÉBITO",
                DataLancamento = extrato.DataLancamento.ToString("d"),
                Valor = extrato.Valor.ToString("N2"),
                Descricao = extrato.Descricao,
                Observacao = extrato.Observacao
            };

            return Json(objetoRetorno);
        }


        [HttpPost]
        public async Task<IActionResult> SalvarExtrato(Guid id, string observacao)
        {

            var extrato = await _context.Extratos.FindAsync(id);

            extrato.Observacao = observacao;

            _context.Extratos.Update(extrato);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
