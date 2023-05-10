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

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;


        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            _mapper = mapper;
        }
        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _geralPersist.add<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                    var EventoRetorno = await _eventoPersist.GetEventoById(evento.Id, false);
                    return _mapper.Map<EventoDto>(EventoRetorno);
                }
                return null;
            }
            catch (Exception e )
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoById(eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                    var EventoRetorno = await _eventoPersist.GetEventoById(evento.Id, false);
                    return _mapper.Map<EventoDto>(EventoRetorno);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoById(eventoId, false);
                
                if (evento == null) throw new Exception("DeleteEvento: Evento para delete não encontrado.");

                _geralPersist.Delete<Evento>(evento);

                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("UpdateEvento " + e.Message);
            }
        }
        
        public async Task<EventoDto[]> GetAllEventosAsync(bool IncluidPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(IncluidPalestrantes);
                if (eventos == null) return null;

                var result = _mapper.Map<EventoDto[]>(eventos);

                return result;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool IncluidPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, IncluidPalestrantes);
                if (eventos == null) return null;

                var result = _mapper.Map<EventoDto[]>(eventos);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDto> GetEventoById(int enveotId, bool IncluidPalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoById(enveotId, IncluidPalestrantes);
                if (evento == null) return null;

                var result = _mapper.Map<EventoDto>(evento);

                return result;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

       
    }
}
