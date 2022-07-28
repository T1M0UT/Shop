using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Orders;

using Shop.Domain.Infrastructure;
using Stripe;
using GetOrder = Shop.Application.Cart.GetOrder;

namespace BonsaiShop.Pages.Checkout;

public class PaymentModel : PageModel
{
    public string PublicKey { get; }

    public PaymentModel(IConfiguration config)
    {
        PublicKey = config["Stripe:PublicKey"];
    }

    public IActionResult OnGet(
        [FromServices] GetCustomerInformation getCustomerInformation)
    {
        var information = getCustomerInformation.Do();

        if (information is null)
        {
            return RedirectToPage("/Checkout/CustomerInformation");
        }

        return Page();
    }

    public async Task<IActionResult> OnPost(
        string stripeEmail,
        string stripeToken,
        [FromServices] GetOrder getOrder,
        [FromServices] CreateOrder createOrder,
        [FromServices] ISessionManager sessionManager)
    {
        var customers = new CustomerService();
        var charges = new ChargeService();

        var cartOrder = getOrder.Do();

        var customer = await customers.CreateAsync(new CustomerCreateOptions
        {
            Email = stripeEmail,
            Source = stripeToken
        });

        var charge = await charges.CreateAsync(new ChargeCreateOptions
        {
            Amount = cartOrder.GetTotalCharge(),
            Description = "Shop purchase",
            Currency = "usd",
            Customer = customer.Id
        });

        var sessionId = HttpContext.Session.Id;

        await createOrder.DoAsync(new CreateOrder.Request
        {
            StripeReference = charge.Id,
            SessionId = sessionId,
            
            FirstName = cartOrder.CustomerInformation.FirstName,
            LastName = cartOrder.CustomerInformation.LastName,
            Email = cartOrder.CustomerInformation.Email,
            PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,
            Address1 = cartOrder.CustomerInformation.Address1,
            Address2 = cartOrder.CustomerInformation.Address2,
            City = cartOrder.CustomerInformation.City,
            PostCode = cartOrder.CustomerInformation.PostCode,
            
            Stocks = cartOrder.Products.Select(x => new CreateOrder.Stock
            {
                StockId = x.StockId,
                Quantity = x.Quantity,
            }).ToList()
        });

        sessionManager.ClearCart();

        return RedirectToPage("/Index");
    }
}