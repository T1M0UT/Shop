using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.Cart;

[Service]
public class AddToCart
{
    private readonly ISessionManager _sessionManager;
    private readonly IStockManager _stockManager;

    public AddToCart(
        ISessionManager sessionManager,
        IStockManager stockManager)
    {
        _sessionManager = sessionManager;
        _stockManager = stockManager;
    }

    public class Request
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
    }

    public async Task<bool> DoAsync(Request request)
    {
        
        // Service responsibility
        if (!_stockManager.EnoughStock(request.StockId, request.Quantity))
        {
            // return not enough stock
            return false;
        }
        
        // Database responsibility to update stock
        await _stockManager
            .PutStockOnHold(request.StockId, request.Quantity, _sessionManager.GetId());

        var stock = _stockManager.GetStockWithProduct(request.StockId);
        
        var cartProduct = new CartProduct
        {
            ProductId = stock.ProductId,           
            ProductName = stock.Product.Name,
            StockId = stock.Id,
            Quantity = request.Quantity,
            Price = stock.Product.Price,
        };
        
        _sessionManager.AddProduct(cartProduct);
        
        return true;
    }
}