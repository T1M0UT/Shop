using Shop.Domain.Infrastructure;

namespace Shop.Application.ProductsAdmin;

[Service]
public class GetProduct
{
    private readonly IProductManager _productManager;

    public GetProduct(IProductManager productManager)
    {
        _productManager = productManager;
    }

    public ProductViewModel? Do(int id) =>
        _productManager.GetProductById(id, p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        });

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}