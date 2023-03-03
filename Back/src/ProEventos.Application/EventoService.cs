using ProEvento.Domain;
using ProEventos.Application.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
        }
        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _geralPersist.add<Evento>(model);
                if (await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventoById(model.Id, false);
                }
                return null;
            }
            catch (Exception e )
            {
                throw new Exception("AddEventos - Erro ao tentar adicionar novo evento " + e.Message);
            }
        }
        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoById(eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;

                _geralPersist.Update(model);

                if (await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventoById(model.Id, false);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("UpdateEvento " + e.Message);
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
        
        public async Task<Evento[]> GetAllEventosAsync(bool IncluidPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(IncluidPalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool IncluidPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, IncluidPalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Evento> GetEventoById(int enveotId, bool IncluidPalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoById(enveotId, IncluidPalestrantes);
                if (evento == null) return null;

                return evento;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

       
    }
}
