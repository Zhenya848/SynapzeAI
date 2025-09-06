using MassTransit;
using PaymentService.Contracts.Messaging;

namespace UserService.Infrastructure.Consumers;

public class UserBoughtTheProductEventConsumer : IConsumer<UserBoughtTheProductEvent>
{
    public async Task Consume(ConsumeContext<UserBoughtTheProductEvent> context)
    {
        var data = context.Message;
    }
}