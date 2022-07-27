using Shop.Domain.Infrastructure;

namespace Shop.Application.Products;

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
                Name = p.Name,
                Description = p.Description,
                Price = p.Price.GetPriceString(),
                
                StockCount = p.Stock.Sum(y => y.Quantity)
            });
    
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int StockCount { get; set; }
    }
}