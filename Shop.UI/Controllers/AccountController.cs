using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BonsaiShop.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}