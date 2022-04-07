using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
         Task<PageList<Evento>> ObterTodosEventosAsync(int userId, PageParams pageParams, bool incluirPalestrantes = false);
         Task<Evento> ObterEventoPorIdAsync(int userId, int eventoId, bool incluirPalestrantes = false);

    } 
}