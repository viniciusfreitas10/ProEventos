using Microsoft.EntityFrameworkCore;
using ProEvento.Persistence;
using ProEvento.Persistence.Context;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;

        public PalestrantePersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
                .Where(p => p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
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
