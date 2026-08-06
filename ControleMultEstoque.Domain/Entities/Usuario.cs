using ControleMultEstoque.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleMultEstoque.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string SenhaHash { get; private set; }
        public PapelUsuario Papel { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime CriadoEm { get; private set; }

        protected Usuario() { }

        // Repare: o construtor recebe "senhaHash" já pronto, não a senha em texto puro.
        // Quem calcula o hash é um serviço fora do Domain (na Infrastructure).
        public Usuario(string nome, string email, string senhaHash, PapelUsuario papel)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (!EmailEhValido(email))
                throw new ArgumentException("O e-mail informado não é válido.");

            if (string.IsNullOrWhiteSpace(senhaHash))
                throw new ArgumentException("O hash de senha é obrigatório.");

            Id = Guid.NewGuid();
            Nome = nome;
            Email = email.Trim().ToLowerInvariant();
            SenhaHash = senhaHash;
            Papel = papel;
            Ativo = true;
            CriadoEm = DateTime.UtcNow;
        }

        public void AlterarSenha(string novaSenhaHash)
        {
            if (string.IsNullOrWhiteSpace(novaSenhaHash))
                throw new ArgumentException("O hash de senha é obrigatório.");

            SenhaHash = novaSenhaHash;
        }

        public void PromoverA(PapelUsuario novoPapel)
        {
            Papel = novoPapel;
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public void Reativar()
        {
            Ativo = true;
        }

        public bool EhAdministradorGeral() => Papel == PapelUsuario.AdministradorGeral;

        private static bool EmailEhValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            // Validação simples de formato. A regra de negócio aqui é só
            // "isso parece um e-mail" — validação mais robusta (envio de
            // confirmação, etc.) fica pra camada Application/Infrastructure.
            return email.Contains('@') && email.Contains('.') && !email.Contains(' ');
        }
    }
}
