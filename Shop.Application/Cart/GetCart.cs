using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart;

[Service]
public class GetCart
{
    private readonly ISessionManager _sessionManager;

    public GetCart(ISessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    public class Response
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public decimal RealPrice { get; set; }
        public int Quantity { get; set; }
        public int StockId { get; set; }
    }
    
    public IEnumerable<Response> Do()
    {
        return _sessionManager
            .GetCart(x => new Response
        {
            Name = x.ProductName,
            Price = x.Price.GetPriceString(),
            RealPrice = x.Price,
            Quantity = x.Quantity,
            StockId = x.StockId
        });
    }
}