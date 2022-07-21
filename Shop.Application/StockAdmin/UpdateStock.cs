using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.StockAdmin;

public class UpdateStock
{
    private readonly ApplicationDbContext _ctx;

    public UpdateStock(ApplicationDbContext ctx)
    {
        _ctx = ctx;
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
            });

        _ctx.Stocks.UpdateRange(stocks);
        await _ctx.SaveChangesAsync();

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