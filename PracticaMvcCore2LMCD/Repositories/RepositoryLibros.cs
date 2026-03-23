using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2LMCD.Data;
using PracticaMvcCore2LMCD.Models;

namespace PracticaMvcCore2LMCD.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;
        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }
        public async Task<List<Libros>> GetLibrosAsync()
        {
            var consulta = from datos in this.context.Libros select datos;
            return await consulta.ToListAsync();
        }
        public async Task<List<Generos>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }
        public async Task<List<Libros>> GetLibrosGeneroAsync(int idGenero)
        {
            return await this.context.Libros.Where(z => z.idGenero == idGenero).ToListAsync();
        }
        public async Task<Libros> FindLibroAsync(int idLibro)
        {
            return await this.context.Libros.FirstOrDefault(z => z.idLibro == idLibro);
        }
        public async Task<int> GetMaxIdCompraAsync()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Pedidos.MaxAsync(p => p.IdPedido) + 1;
            }
        }
        public async Task<int> GetMaxIdFacturaAsync()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Pedidos.MaxAsync(p => p.IdFactura) + 1;
            }
        }
        public async Task FinalizarCompraAsync(List<int> carrito, int idUsuario)
        {
            int idCompra = await GetMaxIdCompraAsync();
            int idFactura = await GetMaxIdFacturaAsync();
            DateTime fecha = DateTime.Now;
            foreach (int idLibro in carrito.Distinct())
            {
                int pedido = await GetMaxIdCompraAsync();
                await this.context.Pedidos.AddAsync(
                    new Pedido
                    {
                        IdPedido = idCompra,
                        IdFactura = idFactura,
                        IdLibro = idLibro,
                        Fecha = fecha,
                        IdUsuario = idUsuario,
                        Cantidad = carrito.Count(id => id == idLibro)
                    });
                await this.context.SaveChangesAsync();

            }


        }
        public async Task<Usuarios> FindUsuarioAsync(int idusuaio)
        {
            return await this.context.Usuarios.FirstOrDefault(u => u.IdUsuario == idusuaio);
        }
        public async Task<Usuarios> LoginUserAsync(string email, string password)
        {
            return await this.context.Usuarios.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
