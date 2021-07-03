using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCsharp.Domain;
using WebCsharp.Persistence.Contexto;
using WebCsharp.Persistence.Contratos;

namespace WebCsharp.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly WebCsharpContext context;
        public EventoPersist(WebCsharpContext context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if(includePalestrantes) 
            {
                query = query
                .Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if(includePalestrantes) 
            {
                query = query
                .Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id).Where(e => e.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
           IQueryable<Evento> query = context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if(includePalestrantes) 
            {
                query = query
                .Include(e => e.PalestrantesEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id).Where(e => e.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }
    }
}