using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEvento.Domain;

namespace ProEventos.Domain
{
    public class Palestrante
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string MiniCurriculo { get; set; }
        public string ImagemUrl{ get; set; }
        public string Telefone { get; set; }
        public string email { get; set; }
        public IEnumerable<RedeSocial> RedeSocials{ get; set; }
        public IEnumerable<PalestranteEvento> PalestrantesEventos { get; set; }
    }


}
