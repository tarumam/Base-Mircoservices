﻿using FluentValidation.Results;
using System.Threading.Tasks;
using BaseProject.Core.Messages;
using MediatR;

namespace BaseProject.Core.Mediatr
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidationResult> SendCommand<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }
        public async Task PublishEvent<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }
    }
}
