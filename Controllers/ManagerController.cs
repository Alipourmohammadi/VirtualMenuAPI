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
    [HttpPost("add-product")]
    public async Task<IActionResult> AddNewProduct([FromBody] ProductIN product)
    {
      
      if (!ModelState.IsValid)
        return BadRequest("Invalid Product");
      if (Request.Form.Files.Count == 0)
        return BadRequest("No file uploaded.");
      try
      {
        var result = await _managerService.AddNewProduct(product, Request.Form.Files[0]);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    [HttpPost("add-category")]
    public async Task<IActionResult> AddNewCategory([FromBody] CategoryIN category)
    {
      //category = Request.Form["json"];
      if (!ModelState.IsValid)
        return BadRequest("Invalid Product");
      if (Request.Form.Files.Count == 0)
        return BadRequest("No file uploaded.");
      try
      {
        var result = await _managerService.AddNewCategory(category, Request.Form.Files[0]);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpDelete("remove-product/{id}")]
    public async Task<IActionResult> RemovePoduct(int id)
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
