using Shop.Database;


namespace Shop.Application.ProductsAdmin;

public class UpdateProduct
{
    private ApplicationDbContext _context;

    public UpdateProduct(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response> DoAsync(Request request)
    {
        if (request == null)
            throw new ArgumentException("No request found");
        
        var product = _context.Products
            .FirstOrDefault(p => p.Id == request.Id);

        if (product == null)
            throw new ArgumentException("No product found");
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        
        await _context.SaveChangesAsync();
        
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