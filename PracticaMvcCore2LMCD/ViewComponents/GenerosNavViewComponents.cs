using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2LMCD.Models;
using PracticaMvcCore2LMCD.Repositories;


namespace PracticaMvcCore2LMCD.ViewComponents
{
    public class GenerosNavViewComponent : ViewComponent
    {
        private RepositoryLibros repo;
        public GenerosNavViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
