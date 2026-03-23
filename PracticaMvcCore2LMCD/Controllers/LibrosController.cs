using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2LMCD.Extensions;
using PracticaMvcCore2LMCD.Models;
using PracticaMvcCore2LMCD.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2LMCD.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult _PerfilUser()
        {
            return PartialView("_PerfilUser");

        }
        public async Task<IActionResult> Libros(int? idGenero)
        {
            List<Libro> libros;
            if (idGenero != null)
            {
                libros = await this.repo.GetLibrosGeneroAsync(idGenero.Value);
            }
            else
            {
                libros = await this.repo.GetLibrosAsync();
            }

            return View(libros);
        }

        public async Task<IActionResult> Details(int idLibro)
        {
            Libro libro = await this.repo.FindLibroAsync(idLibro);
            return View(libro);
        }
        public IActionResult AddLibro(int? idLibro)
        {
            if (idLibro != null)
            {
                List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO") ?? new List<int>();
                carrito.Add(idLibro.Value);
                HttpContext.Session.SetObject("CARRITO", carrito);
                ViewData["totalProd"] = "total de prodeuctos que hay son: " + carrito.Count;
            }
            return RedirectToAction("Carrito");
        }



        public async Task<IActionResult> Carrito()
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito != null)
            {
                List<Libro> libros = await this.repo.GetLibrosCarritoAsync(carrito);
                return View(libros);
            }
            return View(new List<Libro>());
        }
        public async Task<IActionResult> RemoveLibro(int idLibro)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("Carrito");
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.repo.FinalizarCompraAsync(carrito, idusuario);
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("PedidosUsuario");

        }
        public async Task<IActionResult> PedidosUsuario()
        {
            int idUser = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<VistaPedido> vista = await this.repo.GetPedidosUserAsync(idUser);
            return View(vista);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
