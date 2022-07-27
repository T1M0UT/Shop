using Shop.Domain.Infrastructure;

namespace Shop.Application.Products;

[Service]
public class GetProduct
{
    private readonly IStockManager _stockManager;
    private readonly IProductManager _productManager;

    public GetProduct(
        IStockManager stockManager,
        IProductManager productManager)
    {
        _stockManager = stockManager;
        _productManager = productManager;
    }

    public async Task<ProductViewModel?> DoAsync(string name)
    {
        await _stockManager.RetrieveExpiredStockOnHold();

        return _productManager.GetProductByName(name, p => new ProductViewModel
        {
            Name = p.Name,
            Description = p.Description,
            Price = p.Price.GetPriceString(),

            Stock = p.Stock.Select(y => new StockViewModel
            {
                Id = y.Id,
                Description = y.Description,
                Quantity = y.Quantity
            })
        });
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
