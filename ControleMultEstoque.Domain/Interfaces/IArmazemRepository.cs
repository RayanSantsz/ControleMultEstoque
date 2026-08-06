using System;
using System.Collections.Generic;
using System.Text;
using ControleMultEstoque.Domain.Entities;

namespace ControleMultEstoque.Domain.Interfaces
{
    public interface IArmazemRepository : IRepositorioBase<Armazem>
    {
        Task<Armazem?> ObterPorResponsavelAsync(Guid responsavelId);
    }
}
