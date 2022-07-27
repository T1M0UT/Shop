using System.Reflection;
using Shop.Application;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.UI.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegister
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
    {
        var serviceType = typeof(Service);
        var definedTypes = serviceType.Assembly.DefinedTypes;

        var services = definedTypes
            .Where(x => x.GetTypeInfo().GetCustomAttribute<Service>() != null);

        foreach (var service in services)
        {
            serviceCollection.AddTransient(service);
        }
        
        serviceCollection.AddTransient<IStockManager, StockManager>();
        serviceCollection.AddTransient<IProductManager, ProductManager>();
        serviceCollection.AddTransient<IOrderManager, OrderManager>();
        // serviceCollection.AddTransient<IUserManager, UserManager>();
        serviceCollection.AddScoped<ISessionManager, SessionManager>();
        
        return serviceCollection;
    }
}