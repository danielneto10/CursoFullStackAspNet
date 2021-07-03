using System.Threading.Tasks;
using WebCsharp.Domain;

namespace WebCsharp.Persistence.Contratos
{
    public interface IEventoPersist
    {
         //Eventos
         Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
         Task<Evento[]> GetAllEventosAsync(bool includePalestrantes);
         Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes);
    }
}