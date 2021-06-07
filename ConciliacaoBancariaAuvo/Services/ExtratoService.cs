using ConciliacaoBancariaAuvo.Data;
using ConciliacaoBancariaAuvo.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ConciliacaoBancariaAuvo.Services
{
    public class ExtratoService
    {

        private readonly ApplicationDbContext _context;

        public ExtratoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<string> ExtrairLancamentosOfx(string fileContents)
        {
            var lancamentosOfx = new List<string>();

            while (fileContents.IndexOf("<STMTTRN>") > -1)
            {
                var indexInicial = fileContents.IndexOf("<STMTTRN>");
                var indexFinal = fileContents.IndexOf("</STMTTRN>") + 10;
                var intervaloIndex = fileContents.Substring(indexInicial, (indexFinal - indexInicial));
                lancamentosOfx.Add(intervaloIndex);

                fileContents = fileContents.Substring(indexFinal);

            }

            return lancamentosOfx;
        }

        public void MapearSalvarExtratos(List<string> lancamentosOfx)
        {
            var extratosExistentes = _context.Extratos.ToList();
            var extratos = new List<Extrato>();

            foreach (var lancamento in lancamentosOfx)
            {
                var extrato = new Extrato();

                #region ExtrairPosiçoes
                var descricaoInicial = lancamento.IndexOf("<MEMO>");
                var valorFinal = lancamento.IndexOf("<MEMO>") - 68;
                var valorDataInicial = lancamento.IndexOf("<DTPOSTED>") + 10;
                var tipoInicial = lancamento.IndexOf("<TRNTYPE>") + 9;
                var tipoFinal = lancamento.IndexOf("<DTPOSTED>") - tipoInicial;
                var valorInicial = lancamento.IndexOf("<TRNAMT>") + 8;
                #endregion

                extrato.Id = extrato.Id;
                extrato.Tipo = lancamento.ToString().Substring(tipoInicial, tipoFinal);
                extrato.Valor = Convert.ToDecimal(lancamento.ToString().Substring(valorInicial, valorFinal).Replace(".", ","));
                extrato.Descricao = lancamento.ToString().Substring(descricaoInicial, 25).Replace("<MEMO>", "");

                #region ConverterData
                var dataAux = lancamento.Substring(valorDataInicial, 8);

                var dataAno = dataAux.Substring(0, 4);
                var dataMes = dataAux.Substring(4, 2);
                var dataDia = dataAux.Substring(6, 2);

                var dataDiaFormatada = Int32.Parse(dataDia);
                var dataMesFormatada = Int32.Parse(dataMes);
                var dataAnoFormatada = Int32.Parse(dataAno);

                extrato.DataLancamento = new DateTime(dataAnoFormatada, dataMesFormatada, dataDiaFormatada);


                #endregion

                if (!extratosExistentes.Any(x => x.Valor == extrato.Valor && x.DataLancamento == extrato.DataLancamento && x.Descricao == extrato.Descricao && x.Tipo == extrato.Tipo))
                {
                    extratos.Add(new Extrato(extrato.Tipo, extrato.DataLancamento, extrato.Valor, extrato.Descricao));
                }
            }

            _context.Extratos.AddRange(extratos);
            _context.SaveChanges();
        }
    }
}
