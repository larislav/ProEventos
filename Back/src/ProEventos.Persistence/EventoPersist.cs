using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventoPersist
    { 
        private readonly ProEventosContext _context;
        public EventoPersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        public async Task<Evento> ObterEventoPorIdAsync(int userId, int eventoId, bool incluirPalestrantes = false)
        {
            //IQueryable<Evento> query = _context.Eventos.AsNoTracking()
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
            // query = query.AsNoTracking().OrderBy(e=>e.Id)
            //     .Where(e=>e.Id == eventoId);
            query = query.AsNoTracking().OrderBy(e=>e.Id)
                .Where(e=>e.Id == eventoId
                && e.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

           public async Task<Evento[]> ObterTodosEventosAsync(int userId, bool incluirPalestrantes = false)
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
            query = query.AsNoTracking()
                .Where(e =>e.UserId == userId)
                .OrderBy(e=>e.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> ObterTodosEventosPorTemaAsync(int userId, string tema, bool incluirPalestrantes = false)
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
                .Where(e=>e.Tema.ToLower().Contains(tema.ToLower())
                && e.UserId == userId);
            return await query.ToArrayAsync();
        }
    }
}