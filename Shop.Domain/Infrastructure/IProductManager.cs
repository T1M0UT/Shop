using Shop.Domain.Models;

namespace Shop.Domain.Infrastructure;

public interface IProductManager
{
    TResult? GetProductById<TResult>(int id, Func<Product, TResult> selector);

    TResult? GetProductByName<TResult>(string name, Func<Product, TResult> selector);
    IEnumerable<TResult> GetProductsWithStock<TResult>(Func<Product, TResult> selector);

    Task<int> CreateProduct(Product product);
    Task<int> DeleteProductById(int id);
    Task<int> UpdateProduct(Product product);
}