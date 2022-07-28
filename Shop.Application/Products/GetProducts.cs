using Shop.Domain.Infrastructure;

namespace Shop.Application.Products;

[Service]
public class GetProducts
{
    private readonly IStockManager _stockManager;
    private readonly IProductManager _productManager;

    public GetProducts(
        IStockManager stockManager,
        IProductManager productManager)
    {
        _stockManager = stockManager;
        _productManager = productManager;
    }

    public async Task<IEnumerable<ProductViewModel>> DoAsync()
    {
        await _stockManager.RetrieveExpiredStockOnHold();
        
        return _productManager.GetProductsWithStock(p => new ProductViewModel
        {
            Name = p.Name,
            Description = p.Description,
            Price = p.Price.GetPriceString(),

            StockCount = p.Stock.Sum(y => y.Quantity)
        });
    }

    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int StockCount { get; set; }
    }
}