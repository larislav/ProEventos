using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;
        public PalestrantePersist(ProEventosContext context)
        {
            _context = context;
        }
         public async Task<Palestrante> ObterPalestrantePorIdAsync(int palestranteId, bool incluirEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p=>p.RedesSociais); //Usa Include Porque a classe entidade Evento
                                            //Possui IEnumerable de Lote e RedeSocial
            if(incluirEventos)
            {
                query = query.Include(p=>p.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
                //A cada PalestranteEvento que eu tiver, inclua os eventos
            }
            query = query.AsNoTracking().OrderBy(p=>p.Id)
                .Where(p => p.Id == palestranteId);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> ObterTodosPalestrantesAsync(bool incluirEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p=>p.RedesSociais); //Usa Include Porque a classe entidade Evento
                                            //Possui IEnumerable de Lote e RedeSocial
            if(incluirEventos)
            {
                query = query.Include(p=>p.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
                //A cada PalestranteEvento que eu tiver, inclua os eventos
            }
            query = query.OrderBy(p=>p.Id);
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante[]> ObterTodosPalestrantesPorNomeAsync(string nome, bool incluirEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p=>p.RedesSociais); //Usa Include Porque a classe entidade Evento
                                            //Possui IEnumerable de Lote e RedeSocial
            if(incluirEventos)
            {
                query = query.Include(p=>p.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
                //A cada PalestranteEvento que eu tiver, inclua os eventos
            }
            query = query.OrderBy(p=>p.Id)
                .Where(p => p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()) ||
                        p.User.UltimoNome.ToLower().Contains(nome.ToLower()));
            return await query.ToArrayAsync();
        }
    }
}