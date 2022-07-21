using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Enums;

namespace Shop.Application.OrdersAdmin;

public class GetOrder
{
    private readonly ApplicationDbContext _ctx;

    public GetOrder(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public class Response
    {
        public int Id { get; set; }
        public string OrderReference { get; set; }
        public string StripeReference { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        
        public IEnumerable<Product> Products { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string StockDescription { get; set; }
    }
    
    public Response? Do(int id) =>
        _ctx.Orders 
            .Include(x => x.OrderStocks)
            .ThenInclude(x => x.Stock)
            .ThenInclude(x => x.Product)
            .ToList()
            .Where(x => x.Id == id)
            .Select(x => new Response
            {
                Id = x.Id,
                OrderReference = x.OrderReference,
                StripeReference = x.StripeReference,
                
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                PostCode = x.PostCode,
                
                Products = x.OrderStocks.ToList().Select(y => new Product
                {
                    Name = y.Stock.Product.Name,
                    Description = y.Stock.Product.Description,
                    Quantity = y.Quantity,
                    StockDescription = y.Stock.Description,
                }),
            })
            .FirstOrDefault();
}