using BaseProject.Core.DomainObjects;

namespace BaseProject.Catalog.Domain
{
    public class BluesoftToken : Entity, IAggregateRoot
    {
        public string Token { get; set; }
        public int Executions { get; set; }
    }
}
