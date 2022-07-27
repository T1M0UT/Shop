using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BonsaiShop.Pages.Checkout;

public class CustomerInformationModel : PageModel
{
    private readonly IWebHostEnvironment _env;

    public CustomerInformationModel(IWebHostEnvironment env)
    {
        _env = env;
    }
    [BindProperty]
    public AddCustomerInformation.Request CustomerInformation { get; set; }
    
    public IActionResult OnGet(
        [FromServices] GetCustomerInformation getCustomerInformation)
    {
        var information = getCustomerInformation.Do();

        if (information is null)
        {
            if (_env.IsDevelopment())
            {
                CustomerInformation = new AddCustomerInformation.Request
                {
                    FirstName = "Tim",
                    LastName = "K",
                    Email = "email@email.com",
                    PhoneNumber = "1",
                    Address1 = "-",
                    Address2 = "-",
                    City = "-",
                    PostCode = "*"
                };
            }
            return Page();
        }

        return RedirectToPage("/Checkout/Payment");
    }

    public IActionResult OnPost(
        [FromServices] AddCustomerInformation addCustomerInformation)
    {
        if (!ModelState.IsValid)
            return Page();
        
        addCustomerInformation.Do(CustomerInformation);

        return RedirectToPage("/Checkout/Payment");
    }
}