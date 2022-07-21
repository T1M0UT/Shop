using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.Cart;

public class AddToCart
{
    private readonly ISessionManager _sessionManager;
    private readonly ApplicationDbContext _ctx;

    public AddToCart(ISessionManager sessionManager, ApplicationDbContext ctx)
    {
        _sessionManager = sessionManager;
        _ctx = ctx;
    }

    public class Request
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
    }
    
    public async Task<bool> DoAsync(Request request)
    {
        var currentStocksOnHold = _ctx.StocksOnHold
            .ToList()
            .Where(x => x.SessionId == _sessionManager.GetId());
            //.ToList();
        
        var stockToHold = _ctx.Stocks.FirstOrDefault(x => x.Id == request.StockId);
        
        if (stockToHold.Quantity < request.Quantity)
        {
            // return not enough stock
            return false;
        }

        var stockOnHold = _ctx.StocksOnHold
            .FirstOrDefault(x => x.StockId == request.StockId);

        var expiryDate = DateTime.Now.AddMinutes(20);
        
        if (stockOnHold != null)
        {
            stockOnHold!.Quantity += request.Quantity;
        }
        else
        {
            _ctx.StocksOnHold.Add(new StockOnHold
            {
                StockId = stockToHold.Id,
                SessionId = _sessionManager.GetId(),
                Quantity = request.Quantity,
                ExpiryDate = expiryDate
            });
        }

        stockToHold.Quantity -= request.Quantity;

        foreach (var stock in currentStocksOnHold)
        {
            stock.ExpiryDate = expiryDate;
        }
        
        await _ctx.SaveChangesAsync();
        
        _sessionManager.AddProduct(request.StockId, request.Quantity);
        
        return true;
    }
}