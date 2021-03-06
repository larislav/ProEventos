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
    public class LoteService : ILoteService
    {
        private readonly ILotePersist _lotePersist; 
        private readonly IGeralPersist _geralPersist; 
        private readonly IMapper _mapper; 
        public LoteService(ILotePersist lotePersist, IGeralPersist geralPersist,
                            IMapper mapper)
        {
            _lotePersist = lotePersist;
            _geralPersist = geralPersist;
            _mapper = mapper;
        } 
        public async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;
                _geralPersist.Add<Lote>(lote);

                await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersist.ObterLotesPorEventoIdAsync(eventoId);
                if(lotes == null) return null;

                foreach (var model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;
                        _mapper.Map(model, lote);

                        _geralPersist.Update<Lote>(lote);
                        await _geralPersist.SaveChangesAsync();
                    }
                } 
                        
                var loteAtualizado = await _lotePersist.ObterLotesPorEventoIdAsync(eventoId);
                 
                return _mapper.Map<LoteDto[]>(loteAtualizado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.ObterLotePorIdsAsync(eventoId, loteId);
                if(lote == null) throw new Exception("Lote para delete n??o foi encontrado");

                _geralPersist.Delete<Lote>(lote);
                return (await _geralPersist.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> ObterLotePorIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.ObterLotePorIdsAsync(eventoId, loteId);
                if(lote == null) return null;
                var resultado = _mapper.Map<LoteDto>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> ObterLotesPorEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.ObterLotesPorEventoIdAsync(eventoId);
                if(lotes == null) return null;
                var resultado = _mapper.Map<LoteDto[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}