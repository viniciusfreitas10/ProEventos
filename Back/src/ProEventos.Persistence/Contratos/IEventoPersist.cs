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
        Task<Evento[]> GetAllEventosByTemaAsync(int userId,string tema, bool IncluidPalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(int userId, bool IncluidPalestrantes = false);
        Task<Evento> GetEventoById(int userId, int enveotId, bool IncluidPalestrantes = false);
    }
}
