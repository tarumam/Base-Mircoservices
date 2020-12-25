using System;
using BaseProject.Core.DomainObjects;

namespace BaseProject.Clients.API.Models
{
    public class Client : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public bool Excluido { get; private set; }
        public virtual Address Address { get; private set; }

        // EF Relation
        protected Client() { }

        public Client(Guid id, string name, string email, string cpf)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            Excluido = false;
        }

        public void TrocarEmail(string email)
        {
            Email = new Email(email);
        }

        public void AtribuirEndereco(Address endereco)
        {
            Address = endereco;
        }
    }
}
