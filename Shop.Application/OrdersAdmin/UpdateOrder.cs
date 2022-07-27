using Shop.Domain.Infrastructure;

namespace Shop.Application.OrdersAdmin;

[Service]
public class UpdateOrder
{
    private readonly IOrderManager _orderManager;

    public UpdateOrder(IOrderManager orderManager)
    {
        _orderManager = orderManager;
    }

    public Task<int> DoAsync(int id)
    {
        return _orderManager.AdvanceOrder(id);
    }
}