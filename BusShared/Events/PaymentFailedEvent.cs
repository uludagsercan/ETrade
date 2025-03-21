
namespace BusShared.Events
{
    public record PaymentFailedEvent(Guid OrderId, string Reason);
}