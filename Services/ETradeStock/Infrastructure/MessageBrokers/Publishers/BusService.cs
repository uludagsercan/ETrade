
using Application.MessageBrokers;
using MassTransit;

namespace Infrastructure.MessageBrokers.Publishers
{
    public class BusService(IPublishEndpoint publishEndpoint) : IBusService
    {
        public async Task PublishAsync<T>(T message) where T : class
        {
            CancellationTokenSource cancellationTokenSource = new();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
            await publishEndpoint.Publish(message, pipe =>
            {
                pipe.MessageId = Guid.NewGuid();
            }, cancellationTokenSource.Token);
        }
    }
}