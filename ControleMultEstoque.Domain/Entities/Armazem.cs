using ControleMultEstoque.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using ControleMultEstoque.Domain.Enums;

namespace ControleMultEstoque.Domain.Entities
{
    public class Armazem
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Endereco { get; private set; }
        public int CapacidadeTotal { get; private set; }
        public StatusArmazem Status { get; private set; }
        public Guid ResponsavelId { get; private set; } // Id do Usuario responsável

        // EF Core precisa de um construtor vazio "protected" pra conseguir
        // reconstruir o objeto quando lê do banco. Ele fica protected pra
        // que ninguém no resto do código o use por engano.
        protected Armazem() { }

        // Este é o único jeito "oficial" de criar um Armazem novo.
        // É aqui que moram as regras de validação.
        public Armazem(string nome, string endereco, int capacidadeTotal, Guid responsavelId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do armazém é obrigatório.");

            if (capacidadeTotal <= 0)
                throw new ArgumentException("A capacidade total deve ser maior que zero.");

            Id = Guid.NewGuid();
            Nome = nome;
            Endereco = endereco;
            CapacidadeTotal = capacidadeTotal;
            ResponsavelId = responsavelId;
            Status = StatusArmazem.Ativo;
        }

        // "Comportamentos" da entidade, em vez de setters soltos.
        // Isso é o que se chama de Rich Domain Model.
        public void AlterarResponsavel(Guid novoResponsavelId)
        {
            ResponsavelId = novoResponsavelId;
        }

        public void ColocarEmManutencao()
        {
            if (Status == StatusArmazem.Inativo)
                throw new InvalidOperationException("Não é possível colocar em manutenção um armazém inativo.");

            Status = StatusArmazem.EmManutencao;
        }

        public void Ativar()
        {
            Status = StatusArmazem.Ativo;
        }

        public void Desativar()
        {
            Status = StatusArmazem.Inativo;
        }
    }
}
