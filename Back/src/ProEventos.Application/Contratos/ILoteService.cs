using ProEvento.Application.Dtos;
using ProEvento.Domain;
using ProEventos.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface ILoteService
    {
        Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models);
        Task<bool> DeleteLote(int eventoId, int loteId);
        Task<LoteDto[]> GetAllLotesByEventIdAsync(int eventoId);
        Task<LoteDto> GetLoteByIdAsync(int enveotId, int loteId);
    }
}
