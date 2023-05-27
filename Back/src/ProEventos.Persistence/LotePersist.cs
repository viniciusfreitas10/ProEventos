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
    public class LotePersist : ILotePersist
    {
        private readonly ProEventosContext _context;

        public LotePersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Lote[]> GetAllLotesByEventIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query
                .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }

        public async Task<Lote> GetLoteByIdAsync(int eventoId, int id)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query
                .Where(lote => lote.EventoId == eventoId && lote.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
