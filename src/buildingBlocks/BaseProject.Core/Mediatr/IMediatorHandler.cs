using System.Threading.Tasks;
using BaseProject.Core.Messages;
using FluentValidation.Results;

namespace BaseProject.Core.Mediatr
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
        Task<ValidationResult> SendCommand<T>(T comando) where T : Command;
    }
}
