using Microsoft.EntityFrameworkCore;
using ProEvento.Persistence.Context;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly ProEventosContext _context;
        public UserPersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId); //retorna de acordo com a primary key
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == username.ToLower());
        }
    }
}
