using Shop.Domain.Models;

namespace Shop.Domain.Infrastructure;

public interface ISessionManager
{
    string GetId();
    void AddProduct(CartProduct cartProduct);
    bool RemoveProduct(int stockId, int quantity);
    IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector);
    void AddCustomerInformation(CustomerInformation customer);
    CustomerInformation? GetCustomerInformation();
    void ClearCart();
}