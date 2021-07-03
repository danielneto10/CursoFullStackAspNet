using Microsoft.EntityFrameworkCore;
using WebCsharp.Domain;

namespace WebCsharp.Persistence.Contexto
{
    public class WebCsharpContext : DbContext
    {
        public WebCsharpContext(DbContextOptions<WebCsharpContext> options) : base(options)
        {
            
        }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PalestranteEvento>().HasKey(PE => new {PE.EventoId, PE.PalestranteId});
            modelBuilder.Entity<Evento>()
                    .HasMany(e => e.RedesSociais)
                    .WithOne(rs => rs.Evento)
                    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Palestrante>()
                    .HasMany(e => e.RedesSociais)
                    .WithOne(rs => rs.Palestrante)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}