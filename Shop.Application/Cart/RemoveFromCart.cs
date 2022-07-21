using Shop.Application.Infrastructure;
using Shop.Database;

namespace Shop.Application.Cart;

public class RemoveFromCart
{
    private readonly ISessionManager _sessionManager;
    private readonly ApplicationDbContext _ctx;

    public RemoveFromCart(ISessionManager sessionManager, ApplicationDbContext ctx)
    {
        _sessionManager = sessionManager;
        _ctx = ctx;
    }

    public class Request
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public bool All { get; set; }
    }

    public async Task<bool> DoAsync(Request request)
    {
        var stockOnHold = _ctx.StocksOnHold
            .FirstOrDefault(x=> x.StockId == request.StockId
            && x.SessionId == _sessionManager.GetId());

        var stock = _ctx.Stocks.FirstOrDefault(x => x.Id == request.StockId);

        if (stock is null || stockOnHold is null)
            return false;

        if (request.All)
        {
            stock.Quantity += stockOnHold.Quantity;
            _sessionManager.RemoveProduct(request.StockId, stockOnHold.Quantity);
            stockOnHold.Quantity = 0;
        }
        else
        {
            stock.Quantity += request.Quantity;
            stockOnHold.Quantity -= request.Quantity;
            _sessionManager.RemoveProduct(request.StockId, request.Quantity);
        }

        if (stockOnHold.Quantity <= 0)
        {
            _ctx.StocksOnHold.Remove(stockOnHold);
        }

        await _ctx.SaveChangesAsync();

        return true;
    }
}