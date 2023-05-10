using ProEvento.Application.Dtos;
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
        Task<EventoDto> AddEventos(EventoDto model);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int eventoId);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool IncluidPalestrantes = false);
        Task<EventoDto[]> GetAllEventosAsync(bool IncluidPalestrantes = false);
        Task<EventoDto> GetEventoById(int enveotId, bool IncluidPalestrantes = false);
    }
}
