using MassTransit;
using Microsoft.Extensions.Logging;
using PaymentService.Contracts.Messaging;
using UserService.Application.Commands.SetBalanceToUser;

namespace UserService.Infrastructure.Consumers;

public class UserBoughtTheProductEventConsumer : IConsumer<UserBoughtTheProductEvent>
{
    private readonly SetBalanceToUserHandler _handler;
    private readonly ILogger<UserBoughtTheProductEventConsumer> _logger;
    
    public UserBoughtTheProductEventConsumer(
        SetBalanceToUserHandler handler,
        ILogger<UserBoughtTheProductEventConsumer> logger)
    {
        _handler = handler;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<UserBoughtTheProductEvent> context)
    {
        var command = new SetBalanceToUserCommand(context.Message.UserId, context.Message.Pack);
        
        var result = await _handler.Handle(command, context.CancellationToken);

        if (result.IsFailure)
        {
            var errors = string.Join(", ", result.Error.Select(e => $"{e.Code}: {e.Message}"));
            
            _logger.LogError("Произошла(и) ошибка(и) при сохранении баланса пользователя с id: {id}, " +
                             "Ошибка(и): {error}", command.UserId, errors);
        }
    }
}