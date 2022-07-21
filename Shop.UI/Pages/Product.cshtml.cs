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

    public GetProduct.ProductViewModel? Product { get; set; }
    private string _name { get; set; }

    public async Task<IActionResult> OnGet(string name)
    {
        _name = name;
        Product = await new GetProduct(_ctx).DoAsync(name.Replace("-", " "));
        if (Product is null)
            return RedirectToPage("Index");
        return Page();
    }

    public async Task<IActionResult> OnPost([FromServices] AddToCart addToCart)
    {
        var stockAdded = await addToCart.DoAsync(CartViewModel);

        if (stockAdded)
            return RedirectToPage("Cart");
        // TODO: Add a warning
        return RedirectToAction("OnGet");
    }
}