using System;
using System.Collections.Generic;
using System.Text;

using ControleMultEstoque.Domain.Entities;

namespace ControleMultEstoque.Domain.Interfaces;

public interface IUsuarioRepository : IRepositorioBase<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<bool> EmailJaExisteAsync(string email);
}
