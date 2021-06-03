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

            while (fileContents.IndexOf("<STMTTRN>") > -1)
            {
                var indexInicial = fileContents.IndexOf("<STMTTRN>");
                var indexFinal = fileContents.IndexOf("</STMTTRN>") + 10;

                var intervaloIndex = fileContents.Substring(indexInicial, (indexFinal - indexInicial));

                listaTags.Add(intervaloIndex);

                fileContents = fileContents.Substring(indexFinal);

            }

            return true;
        }

        public void LerArquivo(IFormFile arquivo, string ofxPrefixo)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ofx", ofxPrefixo + arquivo.FileName);

            System.IO.File.Exists(path);

        }

    }
}
