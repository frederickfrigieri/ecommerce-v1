﻿using System.Threading.Tasks;
using ECommerce.Core.Messages;
using FluentValidation.Results;

namespace ECommerce.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
    }
}