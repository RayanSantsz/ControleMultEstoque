using ControleMultEstoque.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleMultEstoque.Domain.Entities
{
    public class Transferencia
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid ArmazemOrigemId { get; private set; }
        public Guid ArmazemDestinoId { get; private set; }
        public int Quantidade { get; private set; }
        public StatusTransferencia Status { get; private set; }
        public Guid SolicitadoPorId { get; private set; }
        public DateTime CriadaEm { get; private set; }
        public DateTime? ConcluidaEm { get; private set; }
        public string? MotivoCancelamento { get; private set; }

        protected Transferencia() { }

        public Transferencia(Guid produtoId, Guid armazemOrigemId, Guid armazemDestinoId, int quantidade, Guid solicitadoPorId)
        {
            if (armazemOrigemId == armazemDestinoId)
                throw new ArgumentException("O armazém de origem e destino não podem ser o mesmo.");

            if (quantidade <= 0)
                throw new ArgumentException("A quantidade a transferir deve ser maior que zero.");

            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            ArmazemOrigemId = armazemOrigemId;
            ArmazemDestinoId = armazemDestinoId;
            Quantidade = quantidade;
            SolicitadoPorId = solicitadoPorId;
            Status = StatusTransferencia.Pendente;
            CriadaEm = DateTime.UtcNow;
        }

        public void IniciarTransito()
        {
            if (Status != StatusTransferencia.Pendente)
                throw new InvalidOperationException(
                    $"Só é possível iniciar o trânsito de uma transferência Pendente. Status atual: {Status}.");

            Status = StatusTransferencia.EmTransito;
        }

        public void Concluir()
        {
            if (Status != StatusTransferencia.EmTransito)
                throw new InvalidOperationException(
                    $"Só é possível concluir uma transferência Em Trânsito. Status atual: {Status}.");

            Status = StatusTransferencia.Concluida;
            ConcluidaEm = DateTime.UtcNow;
        }

        public void Cancelar(string motivo)
        {
            if (Status is StatusTransferencia.Concluida or StatusTransferencia.Cancelada)
                throw new InvalidOperationException(
                    $"Não é possível cancelar uma transferência com status {Status}.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("É obrigatório informar o motivo do cancelamento.");

            Status = StatusTransferencia.Cancelada;
            MotivoCancelamento = motivo;

        }
    }
}
