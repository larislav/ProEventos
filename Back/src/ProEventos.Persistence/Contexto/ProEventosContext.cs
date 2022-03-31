using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contexto
{
    public class ProEventosContext : IdentityDbContext<User, Role, int, 
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        //contexto que vamos utilizar pra fazer a tabela Eventos no SQLite

        public DbSet<Evento> Eventos { get; set; } 
        public DbSet<Lote> Lotes { get; set; } 
        public DbSet<Palestrante> Palestrantes { get; set; } 
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; } 
        public DbSet<RedeSocial> RedesSociais { get; set; } 

         public ProEventosContext(DbContextOptions<ProEventosContext> options) 
            : base(options)
        {
            
        }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new {PE.EventoId, PE.PalestranteId});

            modelBuilder.Entity<UserRole>(userRole => 
                {
                    userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                    userRole.HasOne(ur =>ur.Role) //A UserRole possui 1 Role
                        .WithMany(r => r.UserRoles) //que possui muitas UserRoles
                        .HasForeignKey(ur => ur.RoleId) //que por sua vez tem uma FK RoleId
                        .IsRequired();              //e preciso de uma Role toda vez que eu
                                                    //criar uma UserRole

                    userRole.HasOne(ur =>ur.User) //Qndo eu criar um usuário 
                        .WithMany(r => r.UserRoles) //dentro de UserRole, eu preciso passar
                        .HasForeignKey(ur => ur.UserId) //o UserId
                        .IsRequired();
                }
            );


            modelBuilder.Entity<Evento>()
                .HasMany(e=>e.RedesSociais) //Entidade possui propriedade RedesSociais
                .WithOne(rs=>rs.Evento) //e RedeSocial possui relação com 1 evento 
                .OnDelete(DeleteBehavior.Cascade);//Então ao deletar Evento,
                                //quero setar o comportamento de delete em cascata
            modelBuilder.Entity<Palestrante>()
                .HasMany(e=>e.RedesSociais) //Entidade possui propriedade RedesSociais
                .WithOne(rs=>rs.Palestrante) //e Palestrante possui relação com 1 evento 
                .OnDelete(DeleteBehavior.Cascade);//Então ao deletar Evento,
                                //quero setar o comportamento de delete em cascata
        }

       
    }
}