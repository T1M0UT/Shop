using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Domain.Models;

namespace Shop.Application.Cart;

public class AddToCart
{
    private readonly ISession _session;

    public AddToCart(ISession session)
    {
        _session = session;
    }

    public class Request
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
    }
    
    public void Do(Request request)
    {
        var cartList = new List<CartProduct>();
        var stringObject = _session.GetString("cart");

        if (!string.IsNullOrEmpty(stringObject))
        {
            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
        }

        if (cartList != null && cartList.Any(x => x.StockId == request.StockId))
        {
            cartList.Find(x => x.StockId == request.StockId)!.Quantity += request.Quantity;
        }
        else
        {
            cartList?.Add(new CartProduct
            {
                StockId = request.StockId,
                Quantity = request.Quantity
            });
        }

        stringObject = JsonConvert.SerializeObject(cartList);
        
        _session.SetString("cart", stringObject);
    }
}