using System.Threading.Tasks;
using WebCsharp.Domain;
using WebCsharp.Persistence.Contratos;
using WebCsharp.Persistence.Contexto;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebCsharp.Persistence
{
    public class LotePersist : ILotePersist
    {
        private readonly WebCsharpContext context;
        public LotePersist(WebCsharpContext context)
        {
            this.context = context;
        }

        public async Task<Lote> GetLoteByIdAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = context.Lotes;

            query = query.AsNoTracking().Where(lote => lote.EventoId == eventoId && lote.Id == loteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = context.Lotes;

            query = query.AsNoTracking().Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}