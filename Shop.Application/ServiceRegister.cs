using Shop.Application.Cart;
using Shop.Application.OrdersAdmin;
using Shop.Application.ProductsAdmin;
using Shop.Application.UsersAdmin;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegister
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<Shop.Application.Orders.GetOrder>();
        serviceCollection.AddTransient<GetOrders>();
        serviceCollection.AddTransient<UpdateOrder>();
        
        serviceCollection.AddTransient<AddCustomerInformation>();
        serviceCollection.AddTransient<GetCustomerInformation>();
        serviceCollection.AddTransient<AddToCart>();
        serviceCollection.AddTransient<GetCart>();
        serviceCollection.AddTransient<RemoveFromCart>();
        serviceCollection.AddTransient<Shop.Application.Cart.GetOrder>();
        serviceCollection.AddTransient<Shop.Application.OrdersAdmin.GetOrder>();

        serviceCollection.AddTransient<CreateProduct>();
        serviceCollection.AddTransient<DeleteProduct>();
        serviceCollection.AddTransient<UpdateProduct>();
        serviceCollection.AddTransient<GetProduct>();
        serviceCollection.AddTransient<GetProducts>();

        serviceCollection.AddTransient<CreateUser>();

        return serviceCollection;
    }
}