using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace proyecto_desarmado.Pages.Cuenta;

[AllowAnonymous]
public class LoginModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public IActionResult OnGet()
    {
        return User.Identity?.IsAuthenticated == true ? Destino() : Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Acceso público a datos ficticios: nunca crea usuarios administradores.
        var identidad = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "visitante-demo"),
            new Claim(ClaimTypes.Name, "Visitante"),
            new Claim(ClaimTypes.Role, "Consulta")
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidad),
            new AuthenticationProperties { IsPersistent = false });

        return Destino();
    }

    private IActionResult Destino()
    {
        return Url.IsLocalUrl(ReturnUrl)
            ? LocalRedirect(ReturnUrl!)
            : RedirectToPage("/Index");
    }
}
