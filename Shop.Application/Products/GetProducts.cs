using Microsoft.EntityFrameworkCore;
using Shop.Database; 

namespace Shop.Application.Products;

public class GetProducts
{
    private readonly ApplicationDbContext _ctx;

    public GetProducts(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public IEnumerable<ProductViewModel> Do() =>
        _ctx.Products
            .Include(x => x.Stock)
            .Select(p => new ProductViewModel
            {
                Name = p.Name,
                Description = p.Description,
                Price = $"${p.Price:N2}",
                
                StockCount = p.Stock.Sum(y => y.Quantity)
            })
            .ToList();
    
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int StockCount { get; set; }
    }
}