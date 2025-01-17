using Microsoft.Extensions.Configuration;
using AutoMapper.Configuration;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace DutchTreat.Controllers
{
  public class AccountController: Controller
  {
    private readonly ILogger<AccountController> logger;
    private readonly SignInManager<StoreUser> signInManager;
    private readonly UserManager<StoreUser> userManager;
    private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

    public AccountController(ILogger<AccountController> logger,
      SignInManager<StoreUser> signInManager,
      UserManager<StoreUser> userManager,
      Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
      this.logger = logger;
      this.signInManager = signInManager;
      this.userManager = userManager;
      this.configuration = configuration;
    }
    public IActionResult Login()
    {
      if(this.User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "App");
      }
      return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      if(ModelState.IsValid)
      {
        var result = await signInManager.PasswordSignInAsync(model.Username,
             model.Password,
             model.RememberMe,
             false);

        if(result.Succeeded)
        {
          if(Request.Query.Keys.Contains("ReturnUrl"))
          {
            return Redirect(Request.Query["ReturnUrl"].First());
          }
          else
          {
            return RedirectToAction("Shop", "App");
          }
          
        }
      }


      ModelState.AddModelError("", "Failed to login");
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      await signInManager.SignOutAsync();
      return RedirectToAction("Index", "App");
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
    {
      if(ModelState.IsValid)
      {
        var user = await userManager.FindByNameAsync(model.Username);

        if(user != null)
        {
          var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
          if (result.Succeeded)
          {
            //Create the token
            var claims = new[]
            {
              new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Email),
              new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              //new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              configuration["Tokens:Issuer"],
              configuration["Tokens:Audience"],
              claims,
              expires: DateTime.UtcNow.AddMinutes(30),
              signingCredentials: creds
              );

            var results = new
            {
              token = new JwtSecurityTokenHandler().WriteToken(token),
              expiration = token.ValidTo
            };

            return Created("", results);
          }
        }
        
      }
      return BadRequest();
    }
  }
}
