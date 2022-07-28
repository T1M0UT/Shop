using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Products;


namespace BonsaiShop.Pages;

public class IndexModel : PageModel
{
    public IEnumerable<GetProducts.ProductViewModel> Products {get; set; }

    public async Task OnGet([FromServices] GetProducts getProducts)
    {
        Products = await getProducts.DoAsync();
    }
}