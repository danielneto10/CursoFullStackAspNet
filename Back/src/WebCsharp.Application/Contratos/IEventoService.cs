using System.Threading.Tasks;
using WebCsharp.Domain;

namespace WebCsharp.Application.Contratos
{
    public interface IEventoService
    {
         Task<Evento> AddEvento(Evento model);
         Task<Evento> UpdateEvento(int eventoId, Evento model);
         Task<bool> DeleteEvento(int eventoId);
         Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
         Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
         Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
    }
}