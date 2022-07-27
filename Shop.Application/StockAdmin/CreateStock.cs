using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.StockAdmin;

[Service]
public class CreateStock
{
    private readonly IStockManager _stockManager;

    public CreateStock(IStockManager stockManager)
    {
        _stockManager = stockManager;
    }

    public async Task<Response> DoAsync(Request request)
    {
        var stock = new Stock
        {
            Description = request.Description,
            Quantity = request.Quantity,
            ProductId = request.ProductId
        };

        await _stockManager.CreateStock(stock);

        return new Response
        {
            Id = stock.Id,
            Description = stock.Description,
            Quantity = stock.Quantity,
        };
    }

    public class Request
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}