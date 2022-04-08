using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class RedeSocialPersist : GeralPersist, IRedeSocialPersist
    {
        private readonly ProEventosContext _context;
        public RedeSocialPersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }


        public async Task<RedeSocial> ObterRedeSocialEventoPorIDsAsync(int eventoId, int id)
        {
            IQueryable<RedeSocial> query =_context.RedesSociais;
            query = query.AsNoTracking()
                        .Where(rs => rs.EventoId == eventoId &&
                            rs.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial> ObterRedeSocialPalestrantePorIDsAsync(int palestranteId, int id)
        {
            IQueryable<RedeSocial> query =_context.RedesSociais;
            query = query.AsNoTracking()
                        .Where(rs => rs.PalestranteId == palestranteId &&
                            rs.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial[]> ObterTodosPorEventoIdAsync(int eventoId)
        {
            IQueryable<RedeSocial> query =_context.RedesSociais;

            query = query.AsNoTracking()
                        .Where(rs => rs.EventoId == eventoId);
            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial[]> ObterTodosPorPalestranteIdAsync(int palestranteId)
        {
            IQueryable<RedeSocial> query =_context.RedesSociais;
            query = query.AsNoTracking()
                        .Where(rs => rs.PalestranteId == palestranteId);
            return await query.ToArrayAsync();
        }

    }
}