using ConciliacaoBancariaAuvo.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ConciliacaoBancariaAuvo.Data;
using X.PagedList;
using System;
using System.ComponentModel;
using ConciliacaoBancariaAuvo.Services;
using ConciliacaoBancariaAuvo.Models;

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

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult DetalhesExtrato(Guid id)
        {
            var extrato = _context.Extratos.Find(id);

            return Json(extrato);
        }


        [HttpPost]
        public async Task<IActionResult> SalvarExtrato(Guid id, Extrato extrato, string observacao)
        {
            extrato = await _context.Extratos.FindAsync(id);

            extrato.Observacao = observacao;

            _context.Extratos.Update(extrato);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
