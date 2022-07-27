using Shop.Domain.Enums;
using Shop.Domain.Models;

namespace Shop.Domain.Infrastructure;

public interface IOrderManager
{
    IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector);
    TResult? GetOrderById<TResult>(int id, Func<Order, TResult> selector);
    TResult? GetOrderByReference<TResult>(string reference, Func<Order, TResult> selector);

    bool OrderReferenceExists(string reference);
    
    Task<int> CreateOrder(Order order);
    Task<int> AdvanceOrder(int id);
}