using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
         Task<EventoDto> AddEventos(int userId, EventoDto model);
         Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
         Task<bool> DeleteEvento(int userId, int eventoId);

         Task<EventoDto[]> ObterTodosEventosPorTemaAsync(int userId, string tema, bool incluirPalestrantes = false);
         Task<EventoDto[]> ObterTodosEventosAsync(int userId, bool incluirPalestrantes = false);
         Task<EventoDto> ObterEventoPorIdAsync(int userId, int eventoId, bool incluirPalestrantes = false);
    } 
}