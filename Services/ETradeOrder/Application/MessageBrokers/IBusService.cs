

namespace Application.MessageBrokers
{
    public interface IBusService
    {
        Task PublishAsync<T>(T message) where T : class;
    }
}