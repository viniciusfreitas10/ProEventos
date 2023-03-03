using ProEvento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<Evento> AddEventos(Evento model);
        Task<Evento> UpdateEvento(int eventoId, Evento model);
        Task<bool> DeleteEvento(int eventoId);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool IncluidPalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool IncluidPalestrantes = false);
        Task<Evento> GetEventoById(int enveotId, bool IncluidPalestrantes = false);
    }
}
