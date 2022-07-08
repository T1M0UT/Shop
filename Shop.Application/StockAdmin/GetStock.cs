using Microsoft.EntityFrameworkCore;
using Shop.Database;

namespace Shop.Application.StockAdmin;

public class GetStock
{
    private readonly ApplicationDbContext _ctx;

    public GetStock(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public IEnumerable<ProductViewModel> Do()
    {
        var stock = _ctx.Products
            .Include(x => x.Stock)
            .Select(x => new ProductViewModel {
                Id = x.Id,
                Description = x.Description,
                Stock = x.Stock.Select(y => new StockViewModel
                {
                    Id = y.Id,
                    Description = y.Description,
                    Quantity = y.Quantity
                })
            })
            .ToList();

        return stock;
    }
    
    public class StockViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; } 
        public IEnumerable<StockViewModel> Stock {get; set; }
    }
}