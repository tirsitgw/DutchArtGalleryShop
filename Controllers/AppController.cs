using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
  public class AppController : Controller
  {
    private readonly IMailServices mailService;

    public AppController(IMailServices _mailService)
    {
      mailService = _mailService;
    }
    public IActionResult Index()
    {
      //throw new InvalidOperationException();
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
  }
}
