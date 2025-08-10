using Daylog.Application.Abstractions.Messaging;
using FluentValidation;

namespace Daylog.Application.Abstractions.Behaviors;

internal static class ValidationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            await ValidateAsync(command, validators, cancellationToken);

            var result = await innerHandler.Handle(command, cancellationToken);

            return result;
        }
    }

    internal sealed class CommandHandler<TCommand>(//internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task Handle(TCommand command, CancellationToken cancellationToken)
        {
            await ValidateAsync(command, validators, cancellationToken);

            await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        IEnumerable<IValidator<TQuery>> validators)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken)
        {
            await ValidateAsync(query, validators, cancellationToken);

            var result = await innerHandler.Handle(query, cancellationToken);

            return result;
        }
    }

    private static async Task ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return;
        }

        var validations = validators.Select(validator => validator.ValidateAndThrowAsync(command, cancellationToken));

        await Task.WhenAll(validations);
    }
}
