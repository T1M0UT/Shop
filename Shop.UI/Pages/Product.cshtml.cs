using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Products;
using Shop.Database;


namespace BonsaiShop.Pages;

public class ProductModel : PageModel
{
    [BindProperty]
    public AddToCart.Request CartViewModel { get; set; }

    public GetProduct.ProductViewModel? Product { get; set; }
    private string _name { get; set; }

    public async Task<IActionResult> OnGet(
        string name,
        [FromServices] GetProduct getProduct)
    {
        _name = name;
        Product = await getProduct.DoAsync(name.Replace("-", " "));
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