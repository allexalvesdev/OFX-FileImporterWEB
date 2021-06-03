using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ConciliacaoBancariaAuvo.Entities
{
    public class Extrato
    {
        public Extrato()
        {

        }

        public Extrato(string tipo, string dataLancamento, string descricao, string valor)
        {
            Id = Guid.NewGuid();
            Tipo = tipo;
            DataLancamento = dataLancamento;
            Descricao = descricao;
            Valor = valor;
        }

        public Guid Id { get; set; }

        public IFormFile OfxUpload { get; set; }
        public string Ofx { get; set; }

        public string Tipo { get; set; }
        public string DataLancamento { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string Observacao { get; set; }

    }
}
