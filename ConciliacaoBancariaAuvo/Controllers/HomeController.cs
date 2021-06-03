using ConciliacaoBancariaAuvo.Entities;
using ConciliacaoBancariaAuvo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConciliacaoBancariaAuvo.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Extrato extrato)
        {

            var ofxPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(extrato.OfxUpload, ofxPrefixo))
            {
                return View(extrato);
            }

            extrato.Ofx = ofxPrefixo + extrato.OfxUpload.FileName;

            return RedirectToAction("Index");
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string ofxPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ofx", ofxPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

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
                //<MEMO> CXE     001958 SAQUE
                //</STMTTRN >
                #endregion

                var tam = listaTags.Count;

                for (int i = 0; i < tam; i++)
                {

                    extrato.Id = extrato.Id;

                    extrato.Tipo = listaTags[i].ToString().Substring(19, 5);
                    extrato.DataLancamento = listaTags[i].ToString().Substring(35, 8);
                    extrato.Valor = listaTags[i].ToString().Substring(67, 6);
                    extrato.Descricao = listaTags[i].ToString().Substring(81, 25);

                    extratos.Add(new Extrato(extrato.Tipo,extrato.DataLancamento,extrato.Valor,extrato.Descricao));

                    

                }
            }
            return true;
        }

    }
}
