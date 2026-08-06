using System;
using System.Collections.Generic;
using System.Text;

using ControleMultEstoque.Domain.Entities;

namespace ControleMultEstoque.Domain.Interfaces;

public interface IProdutoRepository : IRepositorioBase<Produto>
{
    Task<Produto?> ObterPorSkuAsync(string sku);
    Task<bool> SkuJaExisteAsync(string sku);
}
