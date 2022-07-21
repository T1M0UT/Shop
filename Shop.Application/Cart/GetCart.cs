using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart;

public class GetCart
{
    private readonly ISessionManager _sessionManager;
    private readonly ApplicationDbContext _ctx;

    public GetCart(ISessionManager sessionManager, ApplicationDbContext ctx)
    {
        _sessionManager = sessionManager;
        _ctx = ctx;
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
        var cartList = _sessionManager.GetCart();
        var response = _ctx.Stocks
            .Include(x => x.Product)
            .ToList()
            .Where(k => cartList.Any(y => y.StockId == k.Id))
            .Select(x => new Response
            {
                Name = x.Product.Name,
                Price = $"${x.Product.Price:N2}",
                RealPrice = x.Product.Price,
                Quantity = cartList.FirstOrDefault(y => y.StockId == x.Id).Quantity,
                StockId = x.Id
            })
            .ToList();

        return response;
    }
}