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

namespace ProEventos.API.Controllers
{ 
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;
        public PalestrantesController(IPalestranteService palestranteService,
                                IWebHostEnvironment hostEnvironment,
                                IAccountService accountService)
        {
            _palestranteService = palestranteService;
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {   //FromQuery: todos os itens do PageParams serão passados via Query (URL)
            //necessário devido a paginação

            //IActionResult permite retornar os status code do http
            try
            {
                var user = User.GetUserId();
                var palestrantes = await _palestranteService.ObterTodosPalestrantesAsync(pageParams, true);
                if(palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet]
        
        public async Task<IActionResult> GetPalestrante()
        {
            try
            {
                var user = User.GetUserId();
                var palestrante = await _palestranteService.ObterPalestrantePorUserIdAsync(user);
                if(palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.ObterPalestrantePorUserIdAsync(userId, false);
                if(palestrante == null)
                    palestrante = await _palestranteService.AddPalestrante(userId, model);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar o palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {
                var userId = User.GetUserId();
                var palestrante = await _palestranteService.UpdatePalestrante(userId, model);
                if(palestrante == null) return NoContent();
                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar o evento. Erro: {ex.Message}");
            }
        }

    }
}
