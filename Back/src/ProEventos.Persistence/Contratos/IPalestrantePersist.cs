using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
        Task<Palestrante[]> ObterTodosPalestrantesPorNomeAsync(string nome, bool incluirEventos);
        Task<Palestrante[]> ObterTodosPalestrantesAsync(bool incluirEventos);
        Task<Palestrante> ObterPalestrantePorIdAsync(int palestranteId, bool incluirEventos);
    }
}