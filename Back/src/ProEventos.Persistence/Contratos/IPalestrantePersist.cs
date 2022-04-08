using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist : IGeralPersist
    {
        Task<PageList<Palestrante>> ObterTodosPalestrantesAsync(PageParams pageParams, bool incluirEventos = false);
        Task<Palestrante> ObterPalestrantePorUserIdAsync(int userId, bool incluirEventos = false);
    } 
}