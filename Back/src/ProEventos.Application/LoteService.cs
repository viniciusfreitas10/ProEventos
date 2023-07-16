using ProEvento.Domain;
using ProEventos.Application.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Persistence.Contratos;
using ProEvento.Application.Dtos;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;


        public LoteService(IGeralPersist geralPersist, ILotePersist lotePersist, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _lotePersist = lotePersist;
            _mapper = mapper;
        }
        
        public async Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetAllLotesByEventIdAsync(eventoId);
                if (lotes == null) return null;

                foreach(var model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        await UpdateLote(lotes, model,eventoId);
                    }
                }
                var LoteRetorno = await _lotePersist.GetAllLotesByEventIdAsync(eventoId);

                return _mapper.Map<LoteDto[]>(LoteRetorno);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task UpdateLote(Lote[] lotes, LoteDto model, int eventoId)
        {
            var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);

            model.EventoId = eventoId;

            _mapper.Map(model, lote);

            _geralPersist.Update<Lote>(lote);

            await _geralPersist.SaveChangesAsync();
        }
        public async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);

                lote.EventoId = eventoId;

                _geralPersist.add<Lote>(lote);

                await _geralPersist.SaveChangesAsync();
               
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdAsync(eventoId,loteId);
                
                if (lote == null) throw new Exception("DeleteLote: lote para delete não encontrado.");

                _geralPersist.Delete<Lote>(lote);

                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("DeleteLote " + e.Message);
            }
        }
        
        public async Task<LoteDto> GetLoteByIdAsync(int enveotId,int loteId)
        {
            try 
            {
                var lote = await _lotePersist.GetLoteByIdAsync(enveotId, loteId);
                if (lote == null) return null;

                var result = _mapper.Map<LoteDto>(lote);

                return result;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<LoteDto[]> GetAllLotesByEventIdAsync(int eventoId)
        {

            var lote = await _lotePersist.GetAllLotesByEventIdAsync(eventoId);
            if (lote == null) return null;

            var result = _mapper.Map<LoteDto[]>(lote);

            return  result;
        }
        
    }
}
