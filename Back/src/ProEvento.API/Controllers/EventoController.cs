using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEvento.API.Models;
using ProEvento.API.Data;
using ProEvento.API.Data.Migrations;

namespace ProEvento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly DataContext dataContext;
        public EventoController(DataContext context) 
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
            return "Exemplo Post";
        } 
        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return dataContext.Eventos.Where(e => e.EventoId == id);
        }
        public int teste { get; set; }
    }
}
