using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Orders;
using Shop.Database;

namespace BonsaiShop.Pages;

public class OrderModel : PageModel
{
    private readonly ApplicationDbContext _ctx;
    
    public GetOrder.Response? Order { get; set; }
    
    public OrderModel(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }
    public void OnGet(string reference)
    {
        Order = new GetOrder(_ctx).Do(reference);
    }
}