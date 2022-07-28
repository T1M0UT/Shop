using System.Linq;
using Microsoft.EntityFrameworkCore;

using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Database;

public class StockManager : IStockManager
{
    private readonly ApplicationDbContext _ctx;

    public StockManager(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public Task<int> CreateStock(Stock stock)
    {
        _ctx.Stocks.Add(stock);
        return _ctx.SaveChangesAsync();
    }

    public Task<int> DeleteStock(int id)
    {
        var stock = _ctx.Stocks.FirstOrDefault(x => x.Id == id);

        _ctx.Stocks.Remove(stock!);
        return _ctx.SaveChangesAsync();
    }

    public Task<int> UpdateStockRange(List<Stock> stocks)
    {
        _ctx.Stocks.UpdateRange(stocks);
        return _ctx.SaveChangesAsync();
    }

    public Task RemoveStockFromHold(int stockId, int quantity, string sessionId)
    {
        var stockOnHold = _ctx.StocksOnHold
            .ToList()
            .FirstOrDefault(x => x.StockId == stockId
                                 && x.SessionId == sessionId);

        var stock = _ctx.Stocks.FirstOrDefault(x => x.Id == stockId);

        stock.Quantity += quantity;
        stockOnHold.Quantity -= quantity;

        if (stockOnHold.Quantity <= 0)
        {
            _ctx.StocksOnHold.Remove(stockOnHold);
        }

        return _ctx.SaveChangesAsync();
    }

    public Task RemoveStockFromHold(string sessionId)
    {
        var stockOnHold = _ctx.StocksOnHold
            .Where(x => x.SessionId == sessionId)
            .ToList();
        
        _ctx.StocksOnHold.RemoveRange(stockOnHold);
        
        return _ctx.SaveChangesAsync();
    }

    public Stock? GetStockWithProduct(int stockId)
    {
        return _ctx.Stocks
            .Include(x => x.Product)
            .FirstOrDefault(x => x.Id == stockId);
    }

    public bool EnoughStock(int stockId, int quantity)
    {
        var stock = _ctx.Stocks
            .FirstOrDefault(x => x.Id == stockId);
        
        if (stock is null)
            return false;
        
        return stock.Quantity >= quantity;
    }

    public Task<int> PutStockOnHold(int stockId, int quantity, string sessionId)
    {
        var stockOnHold = _ctx.StocksOnHold.FirstOrDefault(x => x.StockId == stockId 
                                                                && x.SessionId == sessionId);
        
        var stockToHold = _ctx.Stocks.FirstOrDefault(x => x.Id == stockId);
        if (stockToHold != null) stockToHold.Quantity -= quantity;

        var expiryDate = DateTime.Now.AddMinutes(20);
        
        if (stockOnHold != null)
        {
            stockOnHold!.Quantity += quantity;
        }
        else
        {
            _ctx.StocksOnHold.Add(new StockOnHold
            {
                StockId = stockId,
                SessionId = sessionId,
                Quantity = quantity,
                ExpiryDate = expiryDate
            });
        }

        var currentStocksOnHold = _ctx.StocksOnHold
            .Where(x => x.SessionId == sessionId);

        foreach (var stock in currentStocksOnHold)
        {
            stock.ExpiryDate = expiryDate;
        }

        return _ctx.SaveChangesAsync();
    }

    public Task RetrieveExpiredStockOnHold()
    {
        var stocksOnHold = _ctx.StocksOnHold.ToList().Where(x => x.ExpiryDate < DateTime.Now).ToList();

        if (stocksOnHold.Count <= 0) 
            return Task.CompletedTask;
        
        var stockToReturn = _ctx.Stocks
            .ToList()
            .Where(x => stocksOnHold.Any(y => y.StockId == x.Id))
            .ToList();

        foreach (var stock in stockToReturn)
        {
            stock.Quantity += stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id)!.Quantity;
        }
        
        _ctx.StocksOnHold.RemoveRange(stocksOnHold);

        return _ctx.SaveChangesAsync();
    }
}