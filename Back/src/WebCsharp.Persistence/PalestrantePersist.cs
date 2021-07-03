using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCsharp.Domain;
using WebCsharp.Persistence.Contexto;
using WebCsharp.Persistence.Contratos;

namespace WebCsharp.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly WebCsharpContext context;
        public PalestrantePersist(WebCsharpContext context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = context.Palestrantes
            .Include(p => p.RedesSociais);

            if(includeEventos) 
            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = context.Palestrantes
            .Include(p => p.RedesSociais);

            if(includeEventos) 
            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id).Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }


        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = context.Palestrantes
            .Include(p => p.RedesSociais);

            if(includeEventos) 
            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id).Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}