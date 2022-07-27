using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;

namespace Shop.Application.OrdersAdmin;

[Service]
public class GetOrders
{    
    private readonly IOrderManager _orderManager;
    
    public GetOrders(IOrderManager orderManager)
    {
        _orderManager = orderManager;
    }

    public class Response
    {
        public int Id { get; set; }
        public string OrderReference { get; set; }
        public string Email { get; set; }
    }

    public IEnumerable<Response> Do(int status) =>
        _orderManager.GetOrdersByStatus((OrderStatus)status, 
            x => new Response
            {
                Id = x.Id,
                OrderReference = x.OrderReference,
                Email = x.Email
            });
}