using Microsoft.EntityFrameworkCore;
using ProEvento.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEvento.API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }

        public DataContext(DbContextOptions<DataContext> options) :  base(options) {}
    }
}
