using Microsoft.EntityFrameworkCore;
using ProEventos.API.Models;

namespace ProEventos.API.Data
{
    public class DataContext : DbContext
    {
        //contexto que vamos utilizar pra fazer a tabela Eventos no SQLite

        public DbSet<Evento> Eventos { get; set; } 

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }  
    }
}