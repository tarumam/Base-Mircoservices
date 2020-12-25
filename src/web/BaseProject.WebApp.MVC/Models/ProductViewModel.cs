using System;

namespace BaseProject.WebApp.MVC.Models
{
    public class ProductViewModel
    {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public bool Ativo { get; set; }
            public decimal Valor { get; set; }
            public DateTime DataCadastro { get; set; }
            public string Imagem { get; set; }
            public int QuantidadeEstoque { get; set; }
        }
    }
