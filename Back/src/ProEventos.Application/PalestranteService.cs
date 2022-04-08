using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersist _palestrantePersist; 
        private readonly IMapper _mapper; 
        public PalestranteService(IPalestrantePersist palestrantePersist,
                            IMapper mapper) 
        {
            _palestrantePersist = palestrantePersist;
            _mapper = mapper;
        }
        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;
                _palestrantePersist.Add<Palestrante>(palestrante);
                if(await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteInserido = await _palestrantePersist.ObterPalestrantePorUserIdAsync(userId, false);
                    var resultado = _mapper.Map<PalestranteDto>(palestranteInserido);

                    return resultado;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestrantePersist.ObterPalestrantePorUserIdAsync(userId, false);
                if(palestrante == null) return null;

                model.Id = palestrante.Id;

                _mapper.Map(model, palestrante);

                _palestrantePersist.Update<Palestrante>(palestrante);
                if(await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteAtualizado = await _palestrantePersist.ObterPalestrantePorUserIdAsync(userId, false);
                    var resultado = _mapper.Map<PalestranteDto>(palestranteAtualizado);

                    return resultado;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> ObterPalestrantePorUserIdAsync(int userId, bool incluirEventos = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.ObterPalestrantePorUserIdAsync(userId, incluirEventos);
                if(palestrantes == null) return null;
                var resultado = _mapper.Map<PalestranteDto>(palestrantes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> ObterTodosPalestrantesAsync(PageParams pageParams, bool incluirEventos = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.ObterTodosPalestrantesAsync(pageParams, incluirEventos);
                if(palestrantes == null) return null;
                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);
                
                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPages = palestrantes.TotalPages;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}