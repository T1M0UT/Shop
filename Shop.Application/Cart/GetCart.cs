using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Cart;

public class GetCart
{
    private readonly ISession _session;
    private readonly ApplicationDbContext _ctx;

    public GetCart(ISession session, ApplicationDbContext ctx)
    {
        _session = session;
        _ctx = ctx;
    }

    public class Response
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public int StockId { get; set; }
    }
    
    public Response Do()
    {
        // TODO: Account for multiple items in the cart

        var stringObject =_session.GetString("cart");
        
        var cartProduct = JsonConvert.DeserializeObject<CartProduct>(stringObject);
        
        var response = _ctx.Stocks
            .Include(x => x.Product)
            .Where(x => x.Id == cartProduct.StockId)
            .Select(x => new Response
            {
                Name = x.Product.Name,
                Price = $"$ {x.Product.Price:N2}",
                Quantity = cartProduct.Quantity,
                StockId = x.Id
            })
            .FirstOrDefault();
        
        return response;
    }
}