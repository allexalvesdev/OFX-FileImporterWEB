using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConciliacaoBancariaAuvo.Entities
{
    public class Extrato
    {
        public Extrato()
        {
           
        }

        public Extrato(string tipo, string dataLancamento, string valor, string descricao)
        {
            Id = Guid.NewGuid();
            Tipo = tipo;
            DataLancamento = dataLancamento;
            Valor = valor;
            Descricao = descricao;
        }

        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile OfxUpload { get; set; }
        public string Ofx { get; set; }

        public string Tipo { get; set; }
        public string DataLancamento { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string Observacao { get; set; }

    }
}
