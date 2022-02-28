using System;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist; 
        private readonly IGeralPersist _geralPersist; 
        public EventoService(IEventoPersist eventoPersist, IGeralPersist geralPersist)
        {
            _eventoPersist = eventoPersist;
            _geralPersist = geralPersist;
        }
        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _geralPersist.Add<Evento>(model);
                if(await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.ObterEventoPorIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoPersist.ObterEventoPorIdAsync(eventoId, false);
                if(evento == null) return null;

                model.Id = eventoId;

                _geralPersist.Update(model);
                if(await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.ObterEventoPorIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.ObterEventoPorIdAsync(eventoId, false);
                if(evento == null) throw new Exception("Evento para delete não foi encontrado");

                _geralPersist.Delete<Evento>(evento);
                return (await _geralPersist.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> ObterEventoPorIdAsync(int eventoId, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.ObterEventoPorIdAsync(eventoId, incluirPalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> ObterTodosEventosAsync(bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.ObterTodosEventosAsync(incluirPalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> ObterTodosEventosPorTemaAsync(string tema, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.ObterTodosEventosPorTemaAsync(tema, incluirPalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}