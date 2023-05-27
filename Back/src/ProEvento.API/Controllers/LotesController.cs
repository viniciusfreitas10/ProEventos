using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEvento.Domain;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEvento.Application.Dtos;
using ProEventos.Application.Dtos;

namespace ProEvento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _loteService;
        Logger logger = new Logger();

        public LotesController(ILoteService LotesService) 
        {
            _loteService = LotesService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetLotes(int eventoId)
        {
            try
            {
                logger.Log("GetEvents", "Recuperando eventos no GetEvents", "Info");

                var lotes = await _loteService.GetAllLotesByEventIdAsync(eventoId);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception e)
            {
                logger.Log("GetLotes", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lotes. Erro: {e.Message}" );
            }
        }
        
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                logger.Log("SaveLotes", $"atualizando o lote do lote: ID: {eventoId}", "Info");

                var lote = await _loteService.SaveLote(eventoId, models);
                if (lote == null)
                {
                    return NoContent();
                    logger.Log("SaveLotes", $"Erro ao atualizar o lote do evento: {eventoId} | lote não existe", "Error");
                }
                    return Ok(lote);
            }
            catch (Exception e)
            {
                logger.Log("SaveLotes", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status400BadRequest,
                    $"Erro ao tentar salvar lotes. Erro: {e.Message}");
            }
        }
        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                logger.Log("DeleteLote", $"atualizando lote: ID: {eventoId}", "Info");
                var lote = await _loteService.GetLoteByIdAsync(eventoId,loteId);
                
                if (lote == null) return NoContent();

                return await _loteService.DeleteLote(lote.EventoId, lote.Id) ?  Ok(new {message = "Deletado"}) 
                    : throw new Exception("Ocorreu um erro não específico ao tentar deletar o lote.");
            }
            catch (Exception e)
            {
                logger.Log("DeleteLote", e.Message, "Error");
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar o lote. Erro: {e.Message}");
            }
        }
    }
}
