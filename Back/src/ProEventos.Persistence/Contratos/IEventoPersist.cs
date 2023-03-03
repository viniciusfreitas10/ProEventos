using ProEvento.Domain;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool IncluidPalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool IncluidPalestrantes = false);
        Task<Evento> GetEventoById(int enveotId, bool IncluidPalestrantes = false);
    }
}
