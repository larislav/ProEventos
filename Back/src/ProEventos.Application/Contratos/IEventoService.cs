using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
         Task<EventoDto> AddEventos(EventoDto model);
         Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
         Task<bool> DeleteEvento(int eventoId);

         Task<EventoDto[]> ObterTodosEventosPorTemaAsync(string tema, bool incluirPalestrantes = false);
         Task<EventoDto[]> ObterTodosEventosAsync(bool incluirPalestrantes = false);
         Task<EventoDto> ObterEventoPorIdAsync(int eventoId, bool incluirPalestrantes = false);
    }
}