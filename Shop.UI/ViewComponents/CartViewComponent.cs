using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;
using Shop.UI.Infrastructure;

namespace Shop.UI.ViewComponents;

public class CartViewComponent: ViewComponent
{
    private readonly GetCart _getCart;

    public CartViewComponent(GetCart getCart)
    {
        _getCart = getCart;
    }
    public IViewComponentResult Invoke(string view = "Default")
    {
        if (view == "Small")
        {
            var totalValue = _getCart.Do().Sum(x => x.RealPrice * x.Quantity);
            return View(view, $"${totalValue:N2}");
        }

        return View(view, _getCart.Do());
    }
}