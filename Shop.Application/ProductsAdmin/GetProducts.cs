using Shop.Database; 

namespace Shop.Application.ProductsAdmin;

public class GetProducts
{
    private readonly ApplicationDbContext _ctx;

    public GetProducts(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public IEnumerable<ProductViewModel> Do() =>
        _ctx.Products.ToList().Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
        });
    
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}