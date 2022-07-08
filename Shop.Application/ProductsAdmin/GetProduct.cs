using Shop.Database;

namespace Shop.Application.ProductsAdmin;

public class GetProduct
{
    private readonly ApplicationDbContext _ctx;

    public GetProduct(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public ProductViewModel? Do(int id) =>
        _ctx.Products.ToList().Where(x => x.Id == id).Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            })
            .FirstOrDefault();
    
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}