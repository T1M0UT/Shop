using Shop.Domain.Infrastructure;

namespace Shop.Application.Orders;

[Service]
public class GetOrder
{
    private readonly IOrderManager _orderManager;

    public GetOrder(IOrderManager orderManager)
    {
        _orderManager = orderManager;
    }

    public class Response
    {
        public string OrderReference { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public string TotalValue { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string StockDescription { get; set; }
    }

    public Response? Do(string reference)
    {
        return _orderManager.GetOrderByReference(
            reference, 
            x => new Response
            {
                OrderReference = x.OrderReference,

                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                PostCode = x.PostCode,

                Products = x.OrderStocks.Select(y => new Product
                {
                    Name = y.Stock.Product.Name,
                    Description = y.Stock.Product.Description,
                    Price = y.Stock.Product.Price.GetPriceString(),
                    Quantity = y.Quantity,
                    StockDescription = y.Stock.Description
                }),

                TotalValue = $"$ {x.OrderStocks.Sum(y => y.Stock.Product.Price):N2}"
            });
    }
}