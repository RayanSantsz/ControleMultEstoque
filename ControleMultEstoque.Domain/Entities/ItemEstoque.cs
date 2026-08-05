using System;
using System.Collections.Generic;
using System.Text;

namespace ControleMultEstoque.Domain.Entities
{
    public class ItemEstoque
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid ArmazemId { get; private set; }
        public int Quantidade { get; private set; }
        public string? Lote { get; private set; }
        public DateOnly? DataValidade { get; private set; }
        public DateTime AtualizadoEm { get; private set; }

        protected ItemEstoque() { }

        public ItemEstoque(Guid produtoId, Guid armazemId, int quantidadeInicial, string? lote = null, DateOnly? datavalidade = null)
        {
            if (quantidadeInicial < 0)
                throw new ArgumentException("A quantidade inicial não pode ser negativa.");

            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            ArmazemId = armazemId;
            Quantidade = quantidadeInicial;
            Lote = lote;
            DataValidade = datavalidade;
            AtualizadoEm = DateTime.UtcNow;
        }

        // "Deposito": entrada de mercadoria (compra, devolução, transferencia recebida)
        public void Adicionar(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade a adicionar deve ser maior que zero.");

            Quantidade += quantidade;
            AtualizadoEm = DateTime.UtcNow;
        }

        // "Saque": saída de mercadoria (venda, transferencia enviada, perda)
        public void Remover(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade a remover deve ser maior que zero");

            if (quantidade > Quantidade)
                throw new InvalidOperationException(
                    $"Estoque insuficiente. Disponível: {Quantidade}, solicitado: {quantidade}.");

            Quantidade -= quantidade;
            AtualizadoEm = DateTime.UtcNow;
        }

        public bool EstaVencido()
        {
            return DataValidade.HasValue && DataValidade.Value < DateOnly.FromDateTime(DateTime.UtcNow);
        }

        public bool EstaProximoDoVencimento(int diasDeAntecedencia = 30)
        {
            if (!DataValidade.HasValue) return false;

            var limite = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(diasDeAntecedencia);
            return DataValidade.Value <= limite;

        }
    }
}
