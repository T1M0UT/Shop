using Shop.Domain.Infrastructure;

namespace Shop.Application.StockAdmin;

[Service]
public class DeleteStock
{
    private readonly IStockManager _stockManager;

    public DeleteStock(IStockManager stockManager)
    {
        _stockManager = stockManager;
    }

    public Task<int> DoAsync(int id)
    {
        return _stockManager.DeleteStock(id);
    }
}