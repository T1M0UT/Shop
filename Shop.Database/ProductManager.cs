using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Database;

public class ProductManager : IProductManager
{
    private readonly ApplicationDbContext _ctx;

    public ProductManager(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public TResult? GetProductById<TResult>(int id, Func<Product, TResult> selector)
    {
        return _ctx.Products
            .ToList()
            .Where(x => x.Id == id)
            .Select(selector)
            .FirstOrDefault();
    }

    public TResult? GetProductByName<TResult>(
        string name, 
        Func<Product, TResult> selector)
    {
        return _ctx.Products
            .Include(x => x.Stock)
            .Where(x => x.Name == name)
            .AsEnumerable()
            .Select(selector)
            .FirstOrDefault();
    }

    public IEnumerable<TResult> GetProductsWithStock<TResult>(
        Func<Product, TResult> selector)
    {
        return _ctx.Products
            .Include(x => x.Stock)
            .Select(selector)
            .ToList();
    }

    public Task<int> CreateProduct(Product product)
    {
        _ctx.Products.Add(product);
        return _ctx.SaveChangesAsync();
    }

    public Task<int> DeleteProductById(int id)
    {
        var product = _ctx.Products.FirstOrDefault(x => x.Id == id);
        _ctx.Products.Remove(product!);
        return _ctx.SaveChangesAsync();
    }

    public Task<int> UpdateProduct(Product product)
    {
        _ctx.Products.Update(product);
        return _ctx.SaveChangesAsync();
    }
}