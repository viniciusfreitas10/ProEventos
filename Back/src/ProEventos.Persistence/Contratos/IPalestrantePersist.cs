using ProEvento.Domain;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
        Task<Palestrante[]> GetAllPalestranteByTemaAsync(string nome, bool IncluidEvent);
        Task<Palestrante[]> GetAllPalestranteAsync( bool IncluidEvent);
        Task<Palestrante> GetPalestranteById(int PalestrantetId, bool IncluidEvent);

    }
}
