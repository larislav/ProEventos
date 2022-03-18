using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface ILoteService
    {
         Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models);
         Task<bool> DeleteLote(int eventoId, int loteId);

         Task<LoteDto[]> ObterLotesPorEventoIdAsync(int eventoId);
         Task<LoteDto> ObterLotePorIdsAsync(int eventoId, int loteId);
    }
}