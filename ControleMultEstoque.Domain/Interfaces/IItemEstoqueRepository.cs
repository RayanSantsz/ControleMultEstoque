using System;
using System.Collections.Generic;
using System.Text;

using ControleMultEstoque.Domain.Entities;

namespace ControleMultEstoque.Domain.Interfaces;

public interface IItemEstoqueRepository : IRepositorioBase<ItemEstoque>
{
    Task<ItemEstoque?> ObterPorProdutoEArmazemAsync(Guid produtoId, Guid armazemId);
    Task<IReadOnlyList<ItemEstoque>> ObterPorArmazemAsync(Guid armazemId);
    Task<IReadOnlyList<ItemEstoque>> ObterAbaixoDoMinimoAsync();
}