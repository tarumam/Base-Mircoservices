using System;
using BaseProject.Core.DomainObjects;

namespace BaseProject.Clients.API.Models
{
    public class Address : Entity
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string Neighbohood { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public Guid ClientId { get; private set; }

        // EF Relation
        public Client Client { get; protected set; }

        public Address(string street, string number, string complement, string neighbohood, string zipCode, string city, string state)
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighbohood = neighbohood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }
    }
}