using Shop.Domain.Infrastructure;

namespace Shop.Application.ProductsAdmin;

[Service]
public class GetProducts
{
    private readonly IProductManager _productManager;

    public GetProducts(IProductManager productManager)
    {
        _productManager = productManager;
    }

    public IEnumerable<ProductViewModel> Do() =>
        _productManager.GetProductsWithStock(p => new ProductViewModel
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