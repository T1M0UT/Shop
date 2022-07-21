using Shop.Domain.Models;

namespace Shop.Application.Infrastructure;

public interface ISessionManager
{
    string GetId();
    void AddProduct(int stockId, int quantity);
    bool RemoveProduct(int stockId, int quantity);
    List<CartProduct> GetCart();

    void AddCustomerInformation(CustomerInformation customer);
    CustomerInformation? GetCustomerInformation();
    void ClearCart();
}