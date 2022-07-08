using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;

namespace BonsaiShop.Pages;

public class CartModel : PageModel
{
    public GetCart.Response Cart { get; set; }
    private readonly ApplicationDbContext _ctx;

    public CartModel(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public IActionResult OnGet()
    {
        Cart = new GetCart(HttpContext.Session, _ctx).Do();

        return Page();
    }
}