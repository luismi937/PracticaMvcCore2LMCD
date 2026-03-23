using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2LMCD.Models;
using PracticaMvcCore2LMCD.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2LMCD.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;
        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario user = await this.repo.LoginUserAsync(email, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                    new Claim ("Nombre", user.Nombre),
                    new Claim ("Apellidos", user.Apellido),
                    new Claim ("Email", user.Email),
                    new Claim ("Foto", user.Foto),

                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                string controller = TempData["controller"]?.ToString() ?? "Libros";
                string action = TempData["action"]?.ToString() ?? "Libros";
                return RedirectToAction(action, controller);

            }
            else
            {
                ViewData["Error"] = "Alguna credencial es incorrecta email o contraseña";
                return View();
            }

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("logout", "managed");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
