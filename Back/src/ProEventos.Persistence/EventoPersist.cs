using Microsoft.EntityFrameworkCore;
using ProEvento.Domain;
using ProEvento.Persistence;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Persistence.Contratos;
using ProEvento.Persistence.Context;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;

        public EventoPersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool IncluidPalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedeSociais);
                
            if (IncluidPalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.palestrante);
            }

            query = query.Where(e => e.UserId == userId)
                         .OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool IncluidPalestrantes = false)
        {
            try
            {
                IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedeSociais);

                if (IncluidPalestrantes)
                {
                    query = query.Include(e => e.PalestrantesEventos)
                        .ThenInclude(pe => pe.palestrante);
                }

                query = query.Where(e => e.Tema.ToLower().Contains(tema.ToLower())
                                        && e.UserId == userId).OrderBy(e => e.Id);

                return await query.ToArrayAsync();

            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
       
        public async Task<Evento> GetEventoById(int userId, int eventoId, bool IncluidPalestrantes = false)
        {
            try
            {
                IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedeSociais);

                if (IncluidPalestrantes)
                {
                    query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.palestrante);
                }

                query = query.Where(e => e.Id == eventoId && e.UserId == userId)
                    .AsNoTracking().OrderBy(e => e.Id);

                return await query.FirstOrDefaultAsync();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
