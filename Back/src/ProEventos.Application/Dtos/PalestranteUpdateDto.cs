using System.Collections.Generic;

namespace ProEventos.Application.Dtos
{
    public class PalestranteUpdateDto
    {
        public int Id { get; set; } 
        public int UserId { get; set; }       
        public string MiniCurriculo { get; set; }
        public UserUpdateDto User { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<EventoDto> Eventos { get; set; }
    } 
}