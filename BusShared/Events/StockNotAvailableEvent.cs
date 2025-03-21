
namespace BusShared.Events
{
    public record StockNotAvailableEvent(Guid OrderId, string Reason);
}