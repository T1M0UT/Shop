using Shop.Domain.Infrastructure;

namespace Shop.Application.ProductsAdmin;

[Service]
public class DeleteProduct
{
    private readonly IProductManager _productManager;

    public DeleteProduct(IProductManager productManager)
    {
        _productManager = productManager;
    }

    public async Task<bool> DoAsync(int id)
    {
        if (await _productManager.DeleteProductById(id) <= 0)
            throw new Exception("Failed to delete product");

        return true;
    }
}