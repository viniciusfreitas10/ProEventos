using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEvento.API.Models;

namespace ProEvento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController() 
        { 
            
        }
        public IEnumerable<Evento> evento = new Evento[]
        {
            new Evento()
             {
                EventoId = 1,
                Tema = "Angular + .NET 5",
                Local = "Alagoinhas - BAHIA",
                Lote = "Primeiro Lote",
                QuantidadePessoas = 100,
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                ImagemURL = "foto.png"
             },
            new Evento()
             {
                EventoId = 2,
                Tema = "React + .NET 5",
                Local = "Alagoinhas - BAHIA",
                Lote = "Segundo Lote",
                QuantidadePessoas = 200,
                DataEvento = DateTime.Now.AddDays(3).ToString(),
                ImagemURL = "foto.png"
             }
        };

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return evento;
        }
        [HttpPost]
        public string Post()
        {
            return "Exemplo Post";
        }
        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return evento.Where(e => e.EventoId == id);
        }
        [HttpGet("{id}")]
        public IEnumerable<Evento> GetByDiferenceId(int id)
        {
            return evento.Where(e => e.EventoId != id);
        }
    }
}
