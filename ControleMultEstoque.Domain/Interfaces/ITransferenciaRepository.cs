using ControleMultEstoque.Domain.Entities;
using ControleMultEstoque.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleMultEstoque.Domain.Interfaces;

public interface ITransferenciaRepository : IRepositorioBase<Transferencia>
{
    Task<IReadOnlyList<Transferencia>> ObterPorArmazemAsync(Guid armazemId);
    Task<IReadOnlyList<Transferencia>> ObterPorStatusAsync(StatusTransferencia status);
}