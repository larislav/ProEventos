using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class LotePersist : ILotePersist
    { 
        private readonly ProEventosContext _context;
        public LotePersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        public async Task<Lote> ObterLotePorIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _context.Lotes;
            query = query.AsNoTracking()
                .Where(lote => lote.EventoId == eventoId
                && lote.Id == loteId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> ObterLotesPorEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;
            query = query.AsNoTracking()
                .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> ObterTodosEventosPorTemaAsync(string tema, bool incluirPalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e=>e.Lotes)
                .Include(e=>e.RedesSociais); //Usa Include Porque a classe entidade Evento
                                            //Possui IEnumerable de Lote e RedeSocial
            if(incluirPalestrantes)
            {
                query = query.Include(e=>e.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Palestrante);
                //A cada PalestranteEvento que eu tiver, inclua os palestrantes
            }
            query = query.OrderBy(e=>e.Id)
                .Where(e=>e.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }
    }
}