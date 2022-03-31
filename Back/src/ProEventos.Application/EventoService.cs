using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist; 
        private readonly IGeralPersist _geralPersist; 
        private readonly IMapper _mapper; 
        public EventoService(IEventoPersist eventoPersist, IGeralPersist geralPersist,
                            IMapper mapper) 
        {
            _eventoPersist = eventoPersist;
            _geralPersist = geralPersist;
            _mapper = mapper;
        }
        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;
                _geralPersist.Add<Evento>(evento);
                if(await _geralPersist.SaveChangesAsync())
                {
                    var eventoInserido = await _eventoPersist.ObterEventoPorIdAsync(userId, evento.Id, false);
                    var resultado = _mapper.Map<EventoDto>(eventoInserido);

                    return resultado;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.ObterEventoPorIdAsync(userId, eventoId, false);
                if(evento == null) return null;

                model.Id = eventoId;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);
                if(await _geralPersist.SaveChangesAsync())
                {
                    var eventoAtualizado = await _eventoPersist.ObterEventoPorIdAsync(userId, evento.Id, false);
                    var resultado = _mapper.Map<EventoDto>(eventoAtualizado);

                    return resultado;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.ObterEventoPorIdAsync(userId, eventoId, false);
                if(evento == null) throw new Exception("Evento a ser deletado não foi encontrado");

                _geralPersist.Delete<Evento>(evento);
                return (await _geralPersist.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> ObterEventoPorIdAsync(int userId, int eventoId, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.ObterEventoPorIdAsync(userId, eventoId, incluirPalestrantes);
                if(eventos == null) return null;
                var resultado = _mapper.Map<EventoDto>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> ObterTodosEventosAsync(int userId, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.ObterTodosEventosAsync(userId, incluirPalestrantes);
                if(eventos == null) return null;
                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> ObterTodosEventosPorTemaAsync(int userId, string tema, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.ObterTodosEventosPorTemaAsync(userId, tema, incluirPalestrantes);
                if(eventos == null) return null;
                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}