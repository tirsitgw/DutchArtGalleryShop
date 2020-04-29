using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
  [Route("api/[Controller]")]
  public class ProductsController : Controller
  {
    private readonly IDutchRepository dutchRepository;
    private readonly ILogger<ProductsController> logger;

    public ProductsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger)
    {
      this.dutchRepository = dutchRepository;
      this.logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        return Ok(dutchRepository.GetAllProducts());
      }
      catch(Exception ex)
      {
        logger.LogError($"Failed to get products: {ex}");
        return BadRequest("Failed to get products");
      }
      
    }

  }
}
