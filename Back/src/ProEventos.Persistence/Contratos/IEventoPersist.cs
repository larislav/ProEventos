using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        
         Task<Evento[]> ObterTodosEventosPorTemaAsync(string tema, bool incluirPalestrantes);
         Task<Evento[]> ObterTodosEventosAsync(bool incluirPalestrantes);
         Task<Evento> ObterEventoPorIdAsync(int eventoId, bool incluirPalestrantes);

    }
}