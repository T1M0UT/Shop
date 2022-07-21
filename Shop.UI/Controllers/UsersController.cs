using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.ProductsAdmin;
using Shop.Application.StockAdmin;
using Shop.Application.UsersAdmin;
using Shop.Database;

namespace BonsaiShop.Controllers;

[Route("[controller]")]
[Authorize(Policy = "Admin")]
public class UsersController : Controller
{
    private readonly CreateUser _createUser;

    public UsersController(CreateUser createUser)
    {
        _createUser = createUser;
    }

    public async Task<IActionResult> CreateUser([FromBody] CreateUser.Request request)
    {
        await _createUser.DoAsync(request);
        
        return Ok();
    }
}