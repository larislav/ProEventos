using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : GeralPersist, IPalestrantePersist
    {
        private readonly ProEventosContext _context;
        public PalestrantePersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }
         public async Task<Palestrante> ObterPalestrantePorUserIdAsync(int userId, bool incluirEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(p=>p.RedesSociais); //Usa Include Porque a classe entidade Evento
                                            //Possui IEnumerable de Lote e RedeSocial
            if(incluirEventos)
            {
                query = query.Include(p=>p.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
                //A cada PalestranteEvento que eu tiver, inclua os eventos
            }
            query = query.AsNoTracking().OrderBy(p=>p.Id)
                .Where(p => p.User.Id == userId);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<PageList<Palestrante>> ObterTodosPalestrantesAsync(PageParams pageParams, bool incluirEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(p=>p.RedesSociais); //Usa Include Porque a classe entidade Evento
                                            //Possui IEnumerable de Lote e RedeSocial
            if(incluirEventos)
            {
                query = query.Include(p=>p.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
                //A cada PalestranteEvento que eu tiver, inclua os eventos
            }
            query = query.AsNoTracking()
            .Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                        p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                        p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                        p.User.Funcao == Domain.Enum.Funcao.Palestrante)
                        .OrderBy(p=>p.Id);
            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams._PageSize);
        }
      
    }
}