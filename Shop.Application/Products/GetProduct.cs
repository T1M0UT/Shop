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

    public ProductViewModel? Do(string name) =>
        _ctx.Products
            .Include(x => x.Stock)
            .Where(x => x.Name == name).Select(p => new ProductViewModel
        {
            Name = p.Name,
            Description = p.Description,
            Price = $"$ {p.Price:N2}",
            
            Stock = p.Stock.Select(y => new StockViewModel
            {
                Id = y.Id,
                Description = y.Description,
                InStock = y.Quantity > 0
            })
        })
            .FirstOrDefault();
    
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
        public bool InStock { get; set; }
    }
}
