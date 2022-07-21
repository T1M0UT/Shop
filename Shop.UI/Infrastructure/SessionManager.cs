using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Application.Infrastructure;
using Shop.Domain.Models;

namespace Shop.UI.Infrastructure;

public class SessionManager : ISessionManager
{
    private readonly ISession _session;

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext.Session;
    }

    public string GetId() => _session.Id;

    public void AddProduct(int stockId, int quantity)
    {
        var cartList = new List<CartProduct>();
        var stringObject = _session.GetString("cart");
        
        if (!string.IsNullOrEmpty(stringObject))
        {
            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
        }
        
        if (cartList != null && cartList.Any(x => x.StockId == stockId))
        {
            cartList.Find(x => x.StockId == stockId)!.Quantity += quantity;
        }
        else
        {
            cartList?.Add(new CartProduct
            {
                StockId = stockId,
                Quantity = quantity
            });
        }
        
        stringObject = JsonConvert.SerializeObject(cartList);
        
        _session.SetString("cart", stringObject);

    }

    public bool RemoveProduct(int stockId, int quantity)
    {
        var cartList = new List<CartProduct>();
        var stringObject = _session.GetString("cart");

        if (string.IsNullOrEmpty(stringObject))
            return false;

        cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

        if (!(cartList!.Any(x => x.StockId == stockId)))
            return false;

        var product = cartList.FirstOrDefault(x => x.StockId == stockId);
        product.Quantity -= quantity;
        if (product.Quantity <= 0)
        {
            cartList.Remove(product);
        }
        
        stringObject = JsonConvert.SerializeObject(cartList);
        _session.SetString("cart", stringObject);
        return true;
    }

    public List<CartProduct> GetCart()
    {
        // TODO: Account for multiple items in the cart

        var stringObject = _session.GetString("cart");

        if (string.IsNullOrEmpty(stringObject))
            return new();
        
        return JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
    }

    public void AddCustomerInformation(CustomerInformation customer)
    {
        var stringObject = JsonConvert.SerializeObject(customer);

        _session.SetString("customer-info", stringObject);

    }

    public CustomerInformation? GetCustomerInformation()
    {
        var stringObject = _session.GetString("customer-info");

        if (string.IsNullOrEmpty(stringObject))
            return null;

        var customerInformation = JsonConvert.DeserializeObject<CustomerInformation>(stringObject);
        return customerInformation;
    }

    public void ClearCart()
    {
        _session.Remove("cart");
    }
}