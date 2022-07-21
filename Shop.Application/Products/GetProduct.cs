using Microsoft.EntityFrameworkCore;
using Shop.Database;

namespace Shop.Application.Products;

public class GetProduct
{
    private readonly ApplicationDbContext _ctx;

    public GetProduct(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ProductViewModel?> DoAsync(string name)
    {
        var stocksOnHold = _ctx.StocksOnHold.ToList().Where(x => x.ExpiryDate < DateTime.Now).ToList();

        if (stocksOnHold.Count > 0)
        {
            var stockToReturn = _ctx.Stocks
                .ToList()
                .Where(x => stocksOnHold.Any(y => y.StockId == x.Id))
                .ToList();

            foreach (var stock in stockToReturn)
            {
                stock.Quantity += stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id)!.Quantity;
            }
            
            _ctx.StocksOnHold.RemoveRange(stocksOnHold);

            await _ctx.SaveChangesAsync();
        }
        
        return _ctx.Products
            .Include(x => x.Stock)
            .Where(x => x.Name == name).Select(p => new ProductViewModel
            {
                Name = p.Name,
                Description = p.Description,
                Price = $"${p.Price:N2}",

                Stock = p.Stock.Select(y => new StockViewModel
                {
                    Id = y.Id,
                    Description = y.Description,
                    Quantity = y.Quantity
                })
            })
            .FirstOrDefault();
    }

    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public IEnumerable<StockViewModel> Stock { get; set; }
    }

    public class StockViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

    }
}
