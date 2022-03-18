using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {
        
         Task<Lote[]> ObterLotesPorEventoIdAsync(int eventoId);
         Task<Lote> ObterLotePorIdsAsync(int eventoId, int loteId);

    }
}