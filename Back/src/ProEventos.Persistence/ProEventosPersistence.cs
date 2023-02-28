using Microsoft.EntityFrameworkCore;
using ProEvento.Domain;
using ProEvento.Persistence;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    class ProEventosPersistence : IProEventosPersistence
    {
        private readonly ProEventosContext _context;

        public ProEventosPersistence(ProEventosContext context)
        {
            _context = context;
        }
        public void add<T>(T entity) where T : class
        {
            _context.Add(entity);   
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }
        public async Task<Evento[]> GetAllEventosAsync(bool IncluidPalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedeSociais);
                
            if (IncluidPalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.palestrante);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool IncluidPalestrantes = false)
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

                query = query.Where(e => e.Tema.ToLower().Contains(tema.ToLower()))
                    .OrderBy(e => e.Id);

                return await query.ToArrayAsync();

            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        public async Task<Palestrante[]> GetAllPalestranteAsync(bool IncluidEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
               .Include(e => e.RedeSociais);

            if (IncluidEvento)
            {
                query = query.Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteByTemaAsync(string nome, bool IncluidEvent)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
              .Include(e => e.RedeSociais);

            if (IncluidEvent)
            {
                query = query.Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(e => e.Id)
                .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoById(int eventoId, bool IncluidPalestrantes = false)
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

                query = query.Where(e => e.Id == eventoId)
                    .OrderBy(e => e.Id);

                return await query.FirstOrDefaultAsync();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Palestrante> GetPalestranteById(int PalestrantetId, bool IncluidEvent)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
               .Include(e => e.RedeSociais);

            if (IncluidEvent)
            {
                query = query.Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(e => e.Id)
                .Where(p => p.Id == PalestrantetId);

            return await query.FirstOrDefaultAsync();
        }

    }
}
