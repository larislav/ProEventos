using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
         Task<EventoDto> AddEventos(int userId, EventoDto model);
         Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
         Task<bool> DeleteEvento(int userId, int eventoId);

         Task<PageList<EventoDto>> ObterTodosEventosAsync(int userId, PageParams pageParams, bool incluirPalestrantes = false);
         Task<EventoDto> ObterEventoPorIdAsync(int userId, int eventoId, bool incluirPalestrantes = false);
    } 
}