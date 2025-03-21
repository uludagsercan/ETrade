

using Application.Dtos;

namespace Application.Abstracts
{
    public interface IStockService
    {
        Task<bool> IsStockAvailableAsync(string productId, int quantity);
    }
}