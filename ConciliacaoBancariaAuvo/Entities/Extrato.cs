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

        public Extrato(string tipo, DateTime dataLancamento, decimal valor, string descricao)
        {
            Id = Guid.NewGuid();
            Tipo = tipo;
            DataLancamento = dataLancamento;
            Valor = valor;
            Descricao = descricao;
        }

        public Guid Id { get; set; }

        public string Tipo { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Observacao { get; set; }

    }
}
