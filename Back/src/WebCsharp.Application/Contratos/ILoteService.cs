using System.Threading.Tasks;
using WebCsharp.Application.Dtos;

namespace WebCsharp.Application.Contratos
{
    public interface ILoteService
    {
        Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models);
        Task<bool> DeleteLote(int eventoId, int loteId);
        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDto> GetLoteByIdAsync(int eventoId, int loteId);
    }
}