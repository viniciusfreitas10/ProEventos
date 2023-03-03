using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEvento.Persistence;
using ProEvento.Domain;
using ProEvento.Persistence.Context;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;

namespace ProEvento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        Logger logger = new Logger();

        public EventoController(IEventoService eventoService) 
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                logger.LogInfo("GetEvents", "Recuperando eventos no GetEvents");

                var eventos = await _eventoService.GetAllEventosAsync(true);
                if (eventos == null) return NotFound("Nenhum evento encontrado");

                return Ok(eventos);
            }
            catch (Exception e)
            {
                logger.LogError("GetEvents", e.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {e.Message}" );
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEvent(Evento model)
        {
            try
            {
                logger.LogInfo("AddEvent", $"adicionando evento: ID: {model.Id}");

                var evento = await _eventoService.AddEventos(model);
                if (evento == null) return BadRequest($"Erro ao atualizar o evento: {evento.Id}");
                return Ok(evento);
            }
            catch (Exception e)
            {
                logger.LogError("AddEvent", e.Message);
                return this.StatusCode(StatusCodes.Status400BadRequest,
                    $"Erro ao tentar adicionar eventos. Erro: {e.Message}");
            }
        }
        [HttpPut("{id}/idAt")]
        public async Task<IActionResult> AttEvent(int id, Evento model)
        {
            try
            {
                logger.LogInfo("AttEvent", $"atualizando evento: ID: {model.Id}");

                var evento = await _eventoService.UpdateEvento(id,model);
                if (evento == null) return BadRequest($"Erro ao atualizar o evento: {evento.Id}");
                return Ok(evento);
            }
            catch (Exception e)
            {
                logger.LogError("AttEvent", e.Message);
                return this.StatusCode(StatusCodes.Status400BadRequest,
                    $"Erro ao tentar atualizar eventos. Erro: {e.Message}");
            }
        }
        [HttpDelete("{id}/id")]
        public async Task<IActionResult> DeleteEvent(int id, Evento model)
        {
            try
            {
                logger.LogInfo("DeleteEvent", $"atualizando evento: ID: {model.Id}");

                return await _eventoService.DeleteEvento(id) ?  Ok("Deletado") :  BadRequest($"Erro ao deletar o evento: {id}");
            }
            catch (Exception e)
            {
                logger.LogError("DeleteEvent", e.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar o evento. Erro: {e.Message}");
            }
        }
        [HttpGet("{id}/id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                logger.LogInfo("GetById", $"Recuperando eventos com o id: {id}");
                var evento = await _eventoService.GetEventoById(id, true);
                if (evento == null) return NotFound("Nenhum evento encontrado com esse Id");
                return Ok(evento);
            }
            catch (Exception e)
            {
                logger.LogError("GetById", e.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar evento com Id: {id} - Erro: {e.Message}");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                logger.LogInfo("GetByTema", $"Recuperando eventos com o tema: {tema}");

                var evento = await _eventoService.GetAllEventosByTemaAsync(tema, true);
                if (evento == null) return NotFound("Nenhum evento encontrado com esse tema");
                return Ok(evento);
            }
            catch (Exception e)
            {
                logger.LogError("GetByTema", e.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar evento com o tema: {tema} - Erro: {e.Message}");
            }
        }
    }
}
