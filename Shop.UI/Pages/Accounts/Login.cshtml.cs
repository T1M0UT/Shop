using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BonsaiShop.Pages.Accounts;

public class LoginModel : PageModel
{
    [BindProperty] public LoginViewModel Input { get; set; }

    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginModel(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost()
    {
        var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToPage("/Admin/Index");
        }

        return Page();
    }
}

public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}