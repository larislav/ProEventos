using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDto[]> SalvarPorEvento(int eventoId, RedeSocialDto[] models);
        Task<bool> DeletarPorEvento(int eventoId, int redeSocialId);

        Task<RedeSocialDto[]> SalvarPorPalestrante(int palestranteId, RedeSocialDto[] models);
        Task<bool> DeletarPorPalestrante(int palestranteId, int redeSocialId);

        Task<RedeSocialDto[]> ObterTodosPorEventoIdAsync(int eventoId);

        Task<RedeSocialDto[]> ObterTodosPorPalestranteIdAsync(int palestranteId);

        Task<RedeSocialDto> ObterRedeSocialEventoPorIDsAsync(int eventoId, int redeSocialId);

        Task<RedeSocialDto> ObterRedeSocialPalestrantePorIDsAsync(int palestranteId, int redeSocialId);
    }
}