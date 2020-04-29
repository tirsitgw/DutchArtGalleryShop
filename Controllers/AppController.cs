using DutchTreat.Data;
using DutchTreat.ConfigureServices;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DutchTreat.Controllers
{
  public class AppController : Controller
  {
    private readonly IMailServices mailService;
    private readonly IDutchRepository repository;
   // private readonly DutchContext context;

    public AppController(IMailServices _mailService, IDutchRepository repository)
    {
      mailService = _mailService;
      this.repository = repository;
    }
    public IActionResult Index()
    {
      //throw new InvalidOperationException();
     // var results = repository.Products.ToList();
      return View();
    }

    [HttpGet("contact")]
    public IActionResult Contact()
    {
      return View();
    }

    [HttpPost("contact")]
    public IActionResult Contact(ContactViewModel model)
    {
      if(ModelState.IsValid)
      {
        mailService.SendMessage("eth.tirsit@gmail.com", model.Subject, $"From: {model.Name}-{model.Email}, Message: {model.Message}");
        ViewBag.UserMessage = "Mail sent";
        ModelState.Clear();
      }
      else
      {

      }
      return View();
    }
    public IActionResult About()
    {
      ViewBag.Title = "About Us";
      return View();
    }
    [Authorize]
    public IActionResult Shop()
    {
      //var results = context.Products
      //  .OrderBy(p => p.Category)
      //  .ToList();
      var results = repository.GetAllProducts();
      return View(results);
    }
  }
}
