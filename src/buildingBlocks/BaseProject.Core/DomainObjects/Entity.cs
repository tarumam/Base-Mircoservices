﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseProject.Core.Messages;

namespace BaseProject.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        
        //The property CreatedAt is handled in context COMMIT.
        public DateTime CreatedAt { get; protected set; }
        
        //The property UpdatedAt is handled in context COMMIT.
        public DateTime UpdatedAt { get; protected set; }

        //[Timestamp]
        //public byte[] Timestamp { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        private List<Event> _notificacoes;
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        public void AddEvent(Event evento)
        {
            _notificacoes = _notificacoes ?? new List<Event>();
            _notificacoes.Add(evento);
        }

        public void RemoveEvent(Event eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        public void ClearEvents()
        {
            _notificacoes?.Clear();
        }

        #region Comparações

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        #endregion
    }
}
