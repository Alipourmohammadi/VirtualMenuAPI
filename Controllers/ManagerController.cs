using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Data.Helpers;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.Services.ManagerServices;

namespace VirtualMenuAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Authorize(Roles = UserRoles.Manager)]
  public class ManagerController : ControllerBase
  {
    private readonly string _assetsFolderPath;
    private readonly IManagerService _managerService;

    public ManagerController(IManagerService managerService)
    {
      _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");
      _managerService = managerService;
    }


    //[AllowAnonymous]
    [RequestSizeLimit(1_000_000_000)]
    [HttpPost("add-product")]
    public async Task<IActionResult> AddNewProduct([FromForm] ProductInput product)
    {

      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      try
      {
        var result = await _managerService.AddNewProduct(product);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    [HttpPost("add-category")]
    [RequestSizeLimit(1_000_000_000)]
    public async Task<IActionResult> AddNewCategory([FromForm] CategoryInput category)
    {
      if (!ModelState.IsValid)
        return BadRequest("Invalid Category");
      try
      {
        var result = await _managerService.AddNewCategory(category);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpDelete("remove-product/{id}")]
    public async Task<IActionResult> RemoveProduct(int id)
    {
      try
      {
        await _managerService.RemoveProduct(id);
        return Ok("Product Deleted");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    [HttpDelete("remove-category/{id}")]
    public async Task<IActionResult> RemoveCategory(int id)
    {
      try
      {
        await _managerService.RemoveCategory(id);
        return Ok("Category Deleted");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
