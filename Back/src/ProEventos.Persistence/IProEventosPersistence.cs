using ProEvento.Domain;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    interface IProEventosPersistence
    {
        //GERAL
        void add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(T[] entity) where T : class;

        Task<bool> SaveChangesAsync();

        //EVENTOS

        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool IncluidPalestrantes);
        Task<Evento[]> GetAllEventosAsync(bool IncluidPalestrantes);
        Task<Evento> GetEventoById(int enveotId, bool IncluidPalestrantes);

        //PALESTRANTES
        Task<Palestrante[]> GetAllPalestranteByTemaAsync(string nome, bool IncluidEvent);
        Task<Palestrante[]> GetAllPalestranteAsync( bool IncluidEvent);
        Task<Palestrante> GetPalestranteById(int PalestrantetId, bool IncluidEvent);

    }
}
