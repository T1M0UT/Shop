using Shop.Domain.Models;

namespace Shop.Domain.Infrastructure;

public interface IStockManager
{
    Task<int> CreateStock(Stock stock);
    Task<int> DeleteStock(int id);
    Task<int> UpdateStockRange(List<Stock> stocks);

    Task RemoveStockFromHold(int stockId, int quantity, string sessionId);
    Task RemoveStockFromHold(string sessionId);

    Stock? GetStockWithProduct(int stockId);
    bool EnoughStock(int stockId, int quantity);
    Task<int> PutStockOnHold(int stockId, int quantity, string sessionId);
   
    Task RetrieveExpiredStockOnHold();
}