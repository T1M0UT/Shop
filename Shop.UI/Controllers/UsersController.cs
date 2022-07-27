using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.UI.ViewModels.Admin;

namespace BonsaiShop.Controllers;

[Route("[controller]")]
[Authorize(Policy = "Admin")]
public class UsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsersController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.GetUsersForClaimAsync(new Claim("Role", "Manager"));
        return Ok(users);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserViewModel vm)
    {
        var managerUser = new IdentityUser
        {
            UserName = vm.Username
        };

        var result = await _userManager.CreateAsync(managerUser, vm.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        var managerClaim = new Claim("Role", "Manager");
        
        var result2 = await _userManager.AddClaimAsync(managerUser, managerClaim);
        if (!result2.Succeeded)
        {
            return BadRequest(result2.Errors);
        }
        
        return Ok(managerUser);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
            return BadRequest("User not found");
        
        var deletionResult = await _userManager.DeleteAsync(user);

        // var result = await _userManager.RemoveClaimAsync(
        //     user,
        //     new Claim("Role", "Manager"));
        
        return Ok(deletionResult);
    }
}