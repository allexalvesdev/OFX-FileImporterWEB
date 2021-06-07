using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConciliacaoBancariaAuvo.Entities
{
    public class Extrato
    {
        public Extrato()
        {
           
        }
        public Extrato(Guid id, string observacao)
        {
            Id = id;
            Observacao = observacao;
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

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataLancamento { get; set; }

        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Observacao { get; set; }

    }
}
