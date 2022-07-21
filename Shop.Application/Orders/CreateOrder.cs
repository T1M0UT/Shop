using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Orders;

public class CreateOrder
{
    private readonly ApplicationDbContext _ctx;

    public CreateOrder(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public class Request
    {
        public string StripeReference { get; set; }
        public string SessionId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public List<Stock> Stocks { get; set; }
    }

    public class Stock
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
    }
    
    public async Task<bool> DoAsync(Request request)
    {
        var stockOnHold = _ctx.StocksOnHold
            .ToList()
            .Where(x => x.SessionId == request.SessionId)
            .ToList();
        
        _ctx.StocksOnHold.RemoveRange(stockOnHold);

        var order = new Order
        {
            OrderReference = CreateOrderReference(),
            StripeReference = request.StripeReference,
            
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address1 = request.Address1,
            Address2 = request.Address2,
            City = request.City,
            PostCode = request.PostCode,
            
            OrderStocks = request.Stocks.Select(x => new OrderStock
            {
                StockId = x.StockId,
                Quantity = x.Quantity
            }).ToList()
        };

        _ctx.Orders.Add(order);

        return await _ctx.SaveChangesAsync() > 0;
    }

    public string CreateOrderReference()
    {
        // Random 12 character string
        string orderReference;
        do {
            orderReference = Guid.NewGuid().ToString().Substring(0, 12);
        } while (_ctx.Orders.Any(x => x.OrderReference == orderReference));
        return orderReference;
        
    }
}