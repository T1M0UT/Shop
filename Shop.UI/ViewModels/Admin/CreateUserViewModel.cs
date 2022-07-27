using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModels.Admin;

public class CreateUserViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}