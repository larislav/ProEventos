using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        
         Task<Evento[]> ObterTodosEventosPorTemaAsync(int userId, string tema, bool incluirPalestrantes);
         Task<Evento[]> ObterTodosEventosAsync(int userId, bool incluirPalestrantes);
         Task<Evento> ObterEventoPorIdAsync(int userId, int eventoId, bool incluirPalestrantes);

    } 
}