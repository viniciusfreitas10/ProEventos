using ProEvento.Domain;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {
        /// <summary>
        /// Método get que retornará uma lista de lotes por evento
        /// </summary>
        /// <param name="enveotId">Id do evento</param>
        /// <returns></returns>
        Task<Lote[]> GetAllLotesByEventIdAsync(int eventoId);

        /// <summary>
        /// Espeficiação dos parâmetros do método, que retornará um lote
        /// </summary>
        /// <param name="enveotId">Id do evento</param>
        /// <param name="id">Codigo chave (Id) do lote</param>
        /// <returns></returns>
        Task<Lote> GetLoteByIdAsync(int enveotId, int id);
    }
}
