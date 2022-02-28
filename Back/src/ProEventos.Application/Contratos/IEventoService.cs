using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
         Task<Evento> AddEventos(Evento model);
         Task<Evento> UpdateEvento(int eventoId, Evento model);
         Task<bool> DeleteEvento(int eventoId);

         Task<Evento[]> ObterTodosEventosPorTemaAsync(string tema, bool incluirPalestrantes = false);
         Task<Evento[]> ObterTodosEventosAsync(bool incluirPalestrantes = false);
         Task<Evento> ObterEventoPorIdAsync(int eventoId, bool incluirPalestrantes = false);
    }
}