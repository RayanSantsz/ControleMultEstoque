using System;
using System.Collections.Generic;
using System.Text;

namespace ControleMultEstoque.Domain.Interfaces
{
    public interface IRepositorioBase<TEntidade> where TEntidade : class
    {
        Task<TEntidade?> ObterPorIdAsync(Guid id);
        Task<IReadOnlyList<TEntidade>> ObterTodosAsync();
        Task AdicionarAsync(TEntidade entidade);
        void Remover(TEntidade entidade);
    }
}
