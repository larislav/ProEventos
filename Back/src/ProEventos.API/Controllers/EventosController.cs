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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.Models;
using ProEventos.API.Helpers;

namespace ProEventos.API.Controllers
{ 
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly string _destino = "Images";
        
        private readonly IEventoService _eventoService;
        private readonly IUtil _util;
        private readonly IAccountService _accountService;
        public EventosController(IEventoService eventoService,
                                IUtil util,
                                IAccountService accountService)
        {
            _eventoService = eventoService;
            _util = util;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {   //FromQuery: todos os itens do PageParams serão passados via Query (URL)
            //necessário devido a paginação

            //IActionResult permite retornar os status code do http
            try
            {
                var user = User.GetUserId();
                var eventos = await _eventoService.ObterTodosEventosAsync(user, pageParams, true);
                if(eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = User.GetUserId();
                var eventos = await _eventoService.ObterEventoPorIdAsync(user, id);
                if(eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var userId = User.GetUserId();
                var eventos = await _eventoService.AddEventos(userId, model);
                if(eventos == null) return BadRequest("Erro ao tentar adicionar o evento. Verifique os dados informados e tente novamente.");

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar o evento. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]  
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var userId = User.GetUserId();
                var evento = await _eventoService.ObterEventoPorIdAsync(userId, eventoId, true);
                if(evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if(file.Length > 0)
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    evento.ImagemURL = await _util.SaveImage(file, _destino);
                }
                var eventoRetorno = await _eventoService.UpdateEvento(userId, eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar upload de foto do evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var userId = User.GetUserId();
                var eventos = await _eventoService.UpdateEvento(userId, id, model);
                if(eventos == null) return BadRequest("Erro ao tentar atualizar o evento. Verifique os dados informados e tente novamente.");

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar o evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
             try
            {
                var userId = User.GetUserId();
                var evento = await _eventoService.ObterEventoPorIdAsync(userId, id, true);
                if(evento == null) return NoContent();

                if(await _eventoService.DeleteEvento(userId, id))
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new {message = "Deletado"});
                }
                else
                {
                    throw new Exception("Ocorreu um problema ao tentar deletar o Evento");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar o evento. Erro: {ex.Message}");
            }
        }

       
    }
}
