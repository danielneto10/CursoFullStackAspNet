using Microsoft.EntityFrameworkCore;
using WebCsharp.API.Models;

namespace WebCsharp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Evento> Eventos { get; set; }
    }
}