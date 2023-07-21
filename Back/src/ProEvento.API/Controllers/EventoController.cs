using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEvento.Application.Dtos;
using Microsoft.AspNetCore.Hosting;
using ProEvento.API.services;

namespace ProEvento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEnvironment;

        Logger logger = new Logger();
        Services services = new Services();

        public EventoController(IEventoService eventoService,
            IWebHostEnvironment hostEnvironment) 
        {
            _eventoService = eventoService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                logger.Log("GetEvents", "Recuperando eventos no GetEvents", "Info");

                var eventos = await _eventoService.GetAllEventosAsync(true);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (Exception e)
            {
                logger.Log("GetEvents", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {e.Message}" );
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEvent(EventoDto model)
        {
            try
            {
                logger.Log("AddEvent", $"adicionando evento: ID: {model.Id}", "Info");

                var evento = await _eventoService.AddEventos(model);
                if (evento == null) return NoContent();
                return Ok(evento);
            }
            catch (Exception e)
            {
                logger.Log("AddEvent", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status400BadRequest,
                    $"Erro ao tentar adicionar eventos. Erro: {e.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                logger.Log("UploadImage", "Realizando o uploade de imagem", "Info");

                var evento = await _eventoService.GetEventoById(eventoId, false);

                if (evento == null) return NoContent(); //ToDo: Refatorar e criar método para verificação se o evento existe e chamar nos controller que hoje são repetidos essa verificação

                var file = Request.Form.Files[0];

                if(file.Length > 0)
                {
                    services.DeleteImage(evento.ImagemURL, _hostEnvironment);
                    evento.ImagemURL = await services.SaveImage(file, _hostEnvironment);
                };

                var EventoRetorno = await _eventoService.UpdateEvento(eventoId, evento);

                return Ok(EventoRetorno);

            }
            catch(Exception e)
            {
                logger.Log("UploadImage", e.StackTrace, "Error");
                return this.StatusCode(StatusCodes.Status400BadRequest,
                    $"Erro ao realizar o upload da imagem do evento: {eventoId}. Erro: {e.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AttEvent(int id, EventoDto model)
        {
            try
            {
                logger.Log("AttEvent", $"atualizando evento: ID: {model.Id}", "Info");

                var evento = await _eventoService.UpdateEvento(id,model);
                if (evento == null)
                {
                    return NoContent();
                    logger.Log("AttEvent", $"Erro ao atualizar o evento: {evento.Id}", "Error");
                }
                    return Ok(evento);
            }
            catch (Exception e)
            {
                logger.Log("AttEvent", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status400BadRequest,
                    $"Erro ao tentar atualizar eventos. Erro: {e.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                logger.Log("DeleteEvent", $"atualizando evento: ID: {id}", "Info");

                var evento = await _eventoService.GetEventoById(id, false);

                if (evento == null) return NoContent(); //ToDo: Refatorar e criar método para verificação se o evento existe e chamar nos controller que hoje são repetidos essa verificação

                if (await _eventoService.DeleteEvento(id)) 
                {
                    services.DeleteImage(evento.ImagemURL, _hostEnvironment);
                    return Ok(new { message = "Deletado" });
                }
                else 
                {
                    throw new Exception("Ocorreu um erro específico ao tentar deletar o evento.");
                }
            }
            catch (Exception e)
            {
                logger.Log("DeleteEvent", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar o evento. Erro: {e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                logger.Log("GetById", $"Recuperando eventos com o id: {id}", "Info");
                var evento = await _eventoService.GetEventoById(id, true);
                if (evento == null) return NoContent();
                return Ok(evento);
            }
            catch (Exception e)
            {
                logger.Log("GetById", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar evento com Id: {id} - Erro: {e.Message}");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                logger.Log("GetByTema", $"Recuperando eventos com o tema: {tema}", "Info");

                var evento = await _eventoService.GetAllEventosByTemaAsync(tema, true);
                if (evento == null) return NoContent();
                return Ok(evento);
            }
            catch (Exception e)
            {
                logger.Log("GetByTema", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar evento com o tema: {tema} - Erro: {e.Message}");
            }
        }
    }
}
