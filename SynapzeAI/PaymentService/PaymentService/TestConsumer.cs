using MassTransit;
using PaymentService.Models;

namespace PaymentService;

public class TestConsumer : IConsumer<Product>
{
    public Task Consume(ConsumeContext<Product> context)
    {
        return Task.CompletedTask;
    }
}