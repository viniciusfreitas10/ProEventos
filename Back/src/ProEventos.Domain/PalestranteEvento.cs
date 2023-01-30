using ProEvento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Domain
{
    public class PalestranteEvento
    {
        public int PalestranteId{ get; set; }
        public Palestrante palestrante { get; set; }
        public int EventoID { get; set; }
        public Evento MyProperty { get; set; }
    }
}
