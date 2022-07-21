using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin;

public class DeleteProduct
{
    private ApplicationDbContext _context;

    public DeleteProduct(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DoAsync(int id)
    {
        var product = _context.Products.FirstOrDefault(x => x.Id == id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}