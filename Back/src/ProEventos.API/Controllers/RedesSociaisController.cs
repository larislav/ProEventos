using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using ProEventos.API.Extensions;

namespace ProEventos.API.Controllers
{ 
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;
        public RedesSociaisController(IRedeSocialService redeSocialService,
                                    IPalestranteService palestranteService,
                                    IEventoService eventoService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            //IActionResult permite retornar os status code do http
            try
            {
                if(! await(AutorEvento(eventoId)))
                    return Unauthorized();

                var redesSociais = await _redeSocialService.ObterTodosPorEventoIdAsync(eventoId);
                if(redesSociais == null) return NoContent();

                return Ok(redesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar redes sociais por evento. Erro: {ex.Message}");
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            //IActionResult permite retornar os status code do http
            try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.ObterPalestrantePorUserIdAsync(userId);
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.ObterTodosPorPalestranteIdAsync(palestrante.Id);
                if(redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar redes sociais por palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if(! await(AutorEvento(eventoId)))
                    return Unauthorized();

                var redeSocial = await _redeSocialService.SalvarPorEvento(eventoId, models);
                if(redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar rede social por evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.ObterPalestrantePorUserIdAsync(userId);
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.SalvarPorPalestrante(palestrante.Id, models);
                if(redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar rede social por palestrante. Erro: {ex.Message}");
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
             try
            {
                if(! await(AutorEvento(eventoId)))
                    return Unauthorized();

                var redeSocial = await _redeSocialService.ObterRedeSocialEventoPorIDsAsync(eventoId, redeSocialId);
                if(redeSocial == null) return NoContent();

                return await _redeSocialService.DeletarPorEvento(eventoId, redeSocial.Id) 
                    ?   Ok(new {message = "Rede social deletada"}) 
                    :   throw new Exception("Ocorreu um problema ao tentar deletar a rede social por evento");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social por evento. Erro: {ex.Message}");
            }
        }

         [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
             try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.ObterPalestrantePorUserIdAsync(userId);
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.ObterRedeSocialPalestrantePorIDsAsync(palestrante.Id, redeSocialId);
                if(redeSocial == null) return NoContent();

                return await _redeSocialService.DeletarPorEvento(palestrante.Id, redeSocial.Id) 
                    ?   Ok(new {message = "Rede social deletada"}) 
                    :   throw new Exception("Ocorreu um problema ao tentar deletar a rede social por palestrante");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social por palestrante. Erro: {ex.Message}");
            }
        }

         [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var user = User.GetUserId();
            var evento = await _eventoService.ObterEventoPorIdAsync(user, eventoId, false);
            if(evento == null) return false;
            
            return true;
        }
    }
}
