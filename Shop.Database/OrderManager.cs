using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Database;

public class OrderManager : IOrderManager
{
    private readonly ApplicationDbContext _ctx;
    
    public OrderManager(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }
    public Task<int> CreateOrder(Order order)
    {
        _ctx.Orders.Add(order);

        return _ctx.SaveChangesAsync();
    }

    public IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector)
    {
        return _ctx.Orders
            .Where(order => order.Status == status)
            .Select(selector)
            .ToList();
    }

    private TResult? GetOrder<TResult>(
        Func<Order, bool> condition,
        Func<Order, TResult> selector)
    {
        return _ctx.Orders
            .Include(x => x.OrderStocks)
                .ThenInclude(x => x.Stock)
                    .ThenInclude(x => x.Product)
            .ToList()
            .Where(condition)
            .Select(selector)
            .FirstOrDefault();
    }

    public TResult? GetOrderById<TResult>(int id, Func<Order, TResult> selector)
    {
        return GetOrder(order => order.Id == id, selector);
    }

    public TResult? GetOrderByReference<TResult>(
        string reference, 
        Func<Order, TResult> selector)
    {
        return GetOrder(order => order.OrderReference == reference, selector);
    }

    public bool OrderReferenceExists(string reference)
    {
        return _ctx.Orders.ToList().Any(x => x.OrderReference == reference);
    }

    public Task<int> AdvanceOrder(int id)
    {
        _ctx.Orders.FirstOrDefault(x => x.Id == id)!.Status++;

        return _ctx.SaveChangesAsync();
    }
}