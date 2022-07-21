using Shop.Database;
using Shop.Domain.Enums;

namespace Shop.Application.OrdersAdmin;

public class GetOrders
{
    private readonly ApplicationDbContext _ctx;

    public GetOrders(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public class Response
    {
        public int Id { get; set; }
        public string OrderReference { get; set; }
        public string Email { get; set; }
    }

    public IEnumerable<Response> Do(int status) =>
        _ctx.Orders
            .ToList()
            .Where(x => x.Status == (OrderStatus)status)
            .Select(x => new Response
            {
                Id = x.Id,
                OrderReference = x.OrderReference,
                Email = x.Email
            })
            .ToList();
}