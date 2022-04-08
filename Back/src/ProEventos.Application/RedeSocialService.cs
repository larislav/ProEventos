using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialPersist _redeSocialPersist; 
        private readonly IMapper _mapper; 
        public RedeSocialService(IRedeSocialPersist redeSocialPersist, 
                            IMapper mapper) 
        {
            _redeSocialPersist = redeSocialPersist;
            _mapper = mapper;
        } 
        public async Task AddRedeSocial(int id, RedeSocialDto model, bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);
                if(isEvento)
                {
                    redeSocial.EventoId = id;
                    redeSocial.PalestranteId = null;
                }
                else
                {
                    redeSocial.PalestranteId = id;
                    redeSocial.EventoId = null;
                }
                
                _redeSocialPersist.Add<RedeSocial>(redeSocial);

                await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SalvarPorEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.ObterTodosPorEventoIdAsync(eventoId);
                if(redesSociais == null) return null;

                foreach (var model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {
                        var redeSocial = redesSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.EventoId = eventoId;
                        _mapper.Map(model, redeSocial);

                        _redeSocialPersist.Update<RedeSocial>(redeSocial);
                        await _redeSocialPersist.SaveChangesAsync();
                    }
                } 
                        
                var RedeSocialAtualizado = await _redeSocialPersist.ObterTodosPorEventoIdAsync(eventoId);
                 
                return _mapper.Map<RedeSocialDto[]>(RedeSocialAtualizado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SalvarPorPalestrante(int palestranteId, RedeSocialDto[] models)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.ObterTodosPorPalestranteIdAsync(palestranteId);
                if(redesSociais == null) return null;

                foreach (var model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else
                    {
                        var redeSocial = redesSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.PalestranteId = palestranteId;
                        _mapper.Map(model, redeSocial);

                        _redeSocialPersist.Update<RedeSocial>(redeSocial);
                        await _redeSocialPersist.SaveChangesAsync();
                    }
                } 
                        
                var RedeSocialAtualizado = await _redeSocialPersist.ObterTodosPorPalestranteIdAsync(palestranteId);
                 
                return _mapper.Map<RedeSocialDto[]>(RedeSocialAtualizado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletarPorEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.ObterRedeSocialEventoPorIDsAsync(eventoId, redeSocialId);
                if(redeSocial == null) throw new Exception("Rede Social por evento para delete não foi encontrada");

                _redeSocialPersist.Delete<RedeSocial>(redeSocial);
                return (await _redeSocialPersist.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletarPorPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.ObterRedeSocialPalestrantePorIDsAsync(palestranteId, redeSocialId);
                if(redeSocial == null) throw new Exception("Rede Social por palestrante para delete não foi encontrada");

                _redeSocialPersist.Delete<RedeSocial>(redeSocial);
                return (await _redeSocialPersist.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<RedeSocialDto> ObterRedeSocialEventoPorIDsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.ObterRedeSocialEventoPorIDsAsync(eventoId, redeSocialId);
                if(redeSocial == null) return null;
                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> ObterRedeSocialPalestrantePorIDsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.ObterRedeSocialPalestrantePorIDsAsync(palestranteId, redeSocialId);
                if(redeSocial == null) return null;
                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> ObterTodosPorEventoIdAsync(int eventoId)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.ObterTodosPorEventoIdAsync(eventoId);
                if(redesSociais == null) return null;
                var resultado = _mapper.Map<RedeSocialDto[]>(redesSociais);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

         public async Task<RedeSocialDto[]> ObterTodosPorPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.ObterTodosPorPalestranteIdAsync(palestranteId);
                if(redesSociais == null) return null;
                var resultado = _mapper.Map<RedeSocialDto[]>(redesSociais);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}