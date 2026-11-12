
using System.Security.Claims;
using ClinicaVet.Data;
using ClinicaVet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly ClinicaContext _db;

    public AuthController(ClinicaContext db) => _db = db;

    public IActionResult Login() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string senha)
    {
        var usuario = await _db.Usuarios.SingleOrDefaultAsync(u => u.Email == email);
        var valido = usuario is not null &&
            new PasswordHasher<Usuario>().VerifyHashedPassword(
                usuario, usuario.SenhaHash, senha) != PasswordVerificationResult.Failed;

        if (!valido)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha invalidos.");
            return View();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario!.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        };
        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(principal, new AuthenticationProperties
        {
            IsPersistent = false,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        });

        return RedirectToAction("Index", "Home");
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
