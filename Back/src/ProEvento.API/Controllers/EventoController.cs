using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEvento.Persistence;
using ProEvento.Domain;

namespace ProEvento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly ProEventosContext dataContext;
        public EventoController(ProEventosContext context) 
        {
            dataContext = context;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return dataContext.Eventos;
        }
        [HttpPost]
        public string Post()
        {
            return "POST";
        } 
        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return dataContext.Eventos.Where(e => e.Id == id);
        }
    }
}
