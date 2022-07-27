using Shop.Domain.Infrastructure;


namespace Shop.Application.ProductsAdmin;

[Service]
public class UpdateProduct
{
    private readonly IProductManager _productManager;

    public UpdateProduct(IProductManager productManager)
    {
        _productManager = productManager;
    }

    public async Task<Response> DoAsync(Request request)
    {
        if (request == null)
            throw new ArgumentException("No request found");

        var product = _productManager.GetProductById(request.Id, x => x);

        if (product == null)
            throw new ArgumentException("No product found");
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;

        await _productManager.UpdateProduct(product);
        
        return new Response
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }
    
    public class Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}