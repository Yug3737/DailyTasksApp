using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyTasksApp.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string password)
    {
        if (ModelState.IsValid)
        {
            var realPassword = _configuration.GetSection("Auth:Password").Value;
            if (string.IsNullOrEmpty(realPassword))
                throw new InvalidOperationException("The system does have a correct password to compare against.");
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Password is required");
                return View("Login");
            }

            byte[] passwordByteArray = Encoding.UTF8.GetBytes(password);
            byte[] realPasswordByteArray = Encoding.UTF8.GetBytes(realPassword);
            if (!CryptographicOperations.FixedTimeEquals(passwordByteArray, realPasswordByteArray))
            {
                ModelState.AddModelError("", "Invalid password. Please try again.");
                return View("Login");
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Yug Patel"),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt. Model State is invalid.");
        return View("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(string returnUrl = null)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("Login");
    }

}