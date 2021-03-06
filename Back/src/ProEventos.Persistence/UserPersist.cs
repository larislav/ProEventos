using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly ProEventosContext _context;
        public UserPersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var resultado = await _context.Users.FindAsync(id);
            return resultado;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var resultado = await _context.Users
            .SingleOrDefaultAsync(x => x.UserName == username.ToLower());
            return resultado;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var resultado = await _context.Users.ToListAsync();
            return resultado;
        }

       
    }
}