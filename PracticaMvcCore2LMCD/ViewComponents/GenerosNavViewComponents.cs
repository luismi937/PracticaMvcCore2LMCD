using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2LMCD.Models;
using PracticaMvcCore2LMCD.Repositories;


namespace PracticaMvcCore2LMCD.ViewComponents
{
    public class GenerosNavViewComponents : ViewComponent
    {
        private RepositoryLibros repo;
        public GenerosNavViewComponents(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Generos> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
