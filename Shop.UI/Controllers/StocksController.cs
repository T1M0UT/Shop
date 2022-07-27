using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.ProductsAdmin;
using Shop.Application.StockAdmin;


namespace BonsaiShop.Controllers;

[Route("[controller]")]
[Authorize(Policy = "Manager")]
public class StocksController : Controller
{
    [HttpGet("")]
    public IActionResult GetStock([FromServices] GetStock getStock) => 
        Ok(getStock.Do());
    
    [HttpPost("")]
    public async Task<IActionResult> CreateStock(
        [FromBody] CreateStock.Request request,
        [FromServices] CreateStock createStock) => 
        Ok(await createStock.DoAsync(request));
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock(
        int id,
        [FromServices] DeleteStock deleteStock) => 
        Ok(await deleteStock.DoAsync(id));
    
    [HttpPut("")]
    public async Task<IActionResult> UpdateStock(
        [FromBody] UpdateStock.Request request,
        [FromServices] UpdateStock updateStock) => 
        Ok(await updateStock.DoAsync(request));

}