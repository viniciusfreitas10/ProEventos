using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEvento.Domain;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEvento.Application.Dtos;
using ProEventos.Application.Dtos;
using ProEvento.API.Helpers;

namespace ProEvento.API.Controllers.Lote
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _loteService;

        public LotesController(ILoteService LotesService)
        {
            _loteService = LotesService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetLotes(int eventoId)
        {
            try
            {
                Logger.Log("GetEvents", "Recuperando eventos no GetEvents", "Info");

                var lotes = await _loteService.GetAllLotesByEventIdAsync(eventoId);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception e)
            {
                Logger.Log("GetLotes", e.Message, "Error");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lotes. Erro: {e.Message}");
            }
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                Logger.Log("SaveLotes", $"atualizando o lote do lote: ID: {eventoId}", "Info");

                var lote = await _loteService.SaveLote(eventoId, models);
                if (lote == null)
                {
                    return NoContent();
                }
                return Ok(lote);
            }
            catch (Exception e)
            {
                Logger.Log("SaveLotes", e.Message, "Error");
                return StatusCode(StatusCodes.Status400BadRequest,
                    $"Erro ao tentar salvar lotes. Erro: {e.Message}");
            }
        }
        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                Logger.Log("DeleteLote", $"atualizando lote: ID: {eventoId}", "Info");
                var lote = await _loteService.GetLoteByIdAsync(eventoId, loteId);

                if (lote == null) return NoContent();

                return await _loteService.DeleteLote(lote.EventoId, lote.Id) ? Ok(new { message = "Deletado" })
                    : throw new Exception("Ocorreu um erro não específico ao tentar deletar o lote.");
            }
            catch (Exception e)
            {
                Logger.Log("DeleteLote", e.Message, "Error");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar o lote. Erro: {e.Message}");
            }
        }
    }
}
