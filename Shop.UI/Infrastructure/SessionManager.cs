using Newtonsoft.Json;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.UI.Infrastructure;

public class SessionManager : ISessionManager
{
    private readonly ISession _session;
    private const string KeyCart = "cart";
    private const string KeyCustomerInformation = "customer-info";

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext.Session;
    }

    public string GetId() => _session.Id;

    public void AddProduct(CartProduct cartProduct)
    {
        var cartList = new List<CartProduct>();
        var stringObject = _session.GetString(KeyCart);
        
        if (!string.IsNullOrEmpty(stringObject))
        {
            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
        }
        
        if (cartList != null && cartList.Any(x => x.StockId == cartProduct.StockId))
        {
            cartList.Find(x => x.StockId == cartProduct.StockId)!.Quantity += cartProduct.Quantity;
        }
        else
        {
            cartList?.Add(cartProduct);
        }
        
        stringObject = JsonConvert.SerializeObject(cartList);
        
        _session.SetString(KeyCart, stringObject);

    }

    public bool RemoveProduct(int stockId, int quantity)
    {
        var stringObject = _session.GetString(KeyCart);

        if (string.IsNullOrEmpty(stringObject))
            return false;

        var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

        if (!(cartList!.Any(x => x.StockId == stockId)))
            return false;

        var product = cartList.FirstOrDefault(x => x.StockId == stockId);
        product.Quantity -= quantity;
        if (product.Quantity <= 0)
        {
            cartList.Remove(product);
        }
        
        stringObject = JsonConvert.SerializeObject(cartList);
        _session.SetString(KeyCart, stringObject);
        return true;
    }

    public IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector)
    {
        var stringObject = _session.GetString(KeyCart);

        if (string.IsNullOrEmpty(stringObject))
            return Enumerable.Empty<TResult>();
        
        var cartList =  JsonConvert.DeserializeObject<IEnumerable<CartProduct>>(stringObject);

        return cartList.Select(selector);
    }

    public void AddCustomerInformation(CustomerInformation customer)
    {
        var stringObject = JsonConvert.SerializeObject(customer);

        _session.SetString(KeyCustomerInformation, stringObject);

    }

    public CustomerInformation? GetCustomerInformation()
    {
        var stringObject = _session.GetString(KeyCustomerInformation);

        if (string.IsNullOrEmpty(stringObject))
            return null;

        var customerInformation = JsonConvert.DeserializeObject<CustomerInformation>(stringObject);
        return customerInformation;
    }

    public void ClearCart()
    {
        _session.Remove(KeyCart);
    }
}