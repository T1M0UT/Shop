using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.StockAdmin;

[Service]
public class UpdateStock
{
    private readonly IStockManager _stockManager;

    public UpdateStock(IStockManager stockManager)
    {
        _stockManager = stockManager;
    }

    public async Task<Response> DoAsync(Request request)
    {
        var stocks = request.Stock
            .Select(stock => new Stock
            {
                Id = stock.Id,
                Description = stock.Description,
                Quantity = stock.Quantity,
                ProductId = stock.ProductId
            })
            .ToList();

        await _stockManager.UpdateStockRange(stocks);

        return new Response
        {
            Stock = request.Stock
        };
    }

    public class StockViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
    
    public class Request
    {
        public IEnumerable<StockViewModel> Stock { get; set; }
    }

    public class Response
    {
        public IEnumerable<StockViewModel> Stock { get; set; }
    }
}