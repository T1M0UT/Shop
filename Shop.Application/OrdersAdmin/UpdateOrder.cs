using Shop.Database;

namespace Shop.Application.OrdersAdmin;

public class UpdateOrder
{
    private readonly ApplicationDbContext _ctx;

    public UpdateOrder(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> DoAsync(int id)
    {
        var order = _ctx.Orders.FirstOrDefault(x => x.Id == id);

        if (order is null)
            return false;
        
        order.Status = order.Status + 1;

        return await _ctx.SaveChangesAsync() > 0;
    }
}