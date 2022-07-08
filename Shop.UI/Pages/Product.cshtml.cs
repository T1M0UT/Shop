using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Products;
using Shop.Database;

namespace BonsaiShop.Pages;

public class ProductModel : PageModel
{
    private readonly ApplicationDbContext _ctx;

    public ProductModel(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    [BindProperty]
    public AddToCart.Request CartViewModel { get; set; }

    public class Test
    {
        public string Id { get; set; }
    }

    public GetProduct.ProductViewModel? Product { get; set; }

    public IActionResult OnGet(string name)
    {
        Product = new GetProduct(_ctx).Do(name.Replace("-", " "));
        if (Product is null)
            return RedirectToPage("Index");
        return Page();
    }

    public IActionResult OnPost()
    {
        new AddToCart(HttpContext.Session).Do(CartViewModel);
        
        return RedirectToPage("Cart");
    }
}