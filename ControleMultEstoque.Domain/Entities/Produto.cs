using System;
using System.Collections.Generic;
using System.Text;

namespace ControleMultEstoque.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; private set; }
        public string Sku { get; private set; }
        public string Nome { get; private set; }
        public string Categoria { get; private set; }
        public string? Descricao { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public int QuantidadeMinima { get; private set; } // limiar pra disparar alerta de estoque baixo
        public bool Ativo { get; private set; }

        protected Produto () { }

        public Produto(string sku, string nome, string categoria, decimal valorUnitario, int quantidadeMinima, string? descricao = null)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("O SKU é obrigatorio.");

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do produto é obrigatório.");

            if (valorUnitario < 0)
                throw new ArgumentException("O valor unitário não pode ser negativo.");

            if (quantidadeMinima < 0)
                throw new ArgumentException("A quantidade mínima não pode ser negativa.");

            Id = Guid.NewGuid();
            Sku = sku.Trim().ToUpperInvariant();
            Nome = nome;
            Categoria = categoria;
            Descricao = descricao;
            ValorUnitario = valorUnitario;
            QuantidadeMinima = quantidadeMinima;
            Ativo = true;
        }

        public void AtualizarQuantidadeMinima(int novaQuantidadeMinima)
        {
            if (novaQuantidadeMinima < 0)
                throw new ArgumentException("A quantidade mínima não pode ser negativa.");

            QuantidadeMinima = novaQuantidadeMinima;
        }

        public void Descontinuar()
        {
            Ativo = false;
        }

        public void Reativar()
        {
            Ativo = true;
        }

    }
}
