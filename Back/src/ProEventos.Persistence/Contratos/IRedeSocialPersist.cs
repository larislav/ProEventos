using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IRedeSocialPersist : IGeralPersist
    {
         Task<RedeSocial> ObterRedeSocialEventoPorIDsAsync(int eventoId, int id);
         Task<RedeSocial> ObterRedeSocialPalestrantePorIDsAsync(int palestranteId, int id);
         Task<RedeSocial[]> ObterTodosPorEventoIdAsync(int eventoId);
         Task<RedeSocial[]> ObterTodosPorPalestranteIdAsync(int palestranteId);
    }
}