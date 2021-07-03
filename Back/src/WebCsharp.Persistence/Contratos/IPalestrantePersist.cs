using System.Threading.Tasks;
using WebCsharp.Domain;

namespace WebCsharp.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
         //Palestrantes
         Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos);
         Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);
         Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos);
    }
}