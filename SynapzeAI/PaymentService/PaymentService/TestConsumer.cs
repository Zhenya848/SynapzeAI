using MassTransit;
using PaymentService.Contracts.Messaging;
using PaymentService.Models;

namespace PaymentService;

public class TestConsumer : IConsumer<Product>
{
    public Task Consume(ConsumeContext<Product> context)
    {
        return Task.CompletedTask;
    }
}