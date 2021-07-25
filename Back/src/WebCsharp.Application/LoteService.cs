using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebCsharp.Application.Contratos;
using WebCsharp.Application.Dtos;
using WebCsharp.Domain;
using WebCsharp.Persistence.Contratos;

namespace WebCsharp.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersist geralPersist;
        private readonly IMapper mapper;
        private readonly ILotePersist lotePersist;
        public LoteService(IGeralPersist geralPersist, ILotePersist lotePersist, IMapper mapper)
        {
            this.lotePersist = lotePersist;
            this.mapper = mapper;
            this.geralPersist = geralPersist;
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await lotePersist.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null) throw new Exception("Lote para delete não encontrado.");

                geralPersist.Delete<Lote>(lote);
                return await geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await lotePersist.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null) return null;

                var resultado = mapper.Map<LoteDto>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                var resultado = mapper.Map<LoteDto[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        await UpdateLote(lotes, model, eventoId);
                    }
                }

                var loteRetorno = await lotePersist.GetLotesByEventoIdAsync(eventoId);
                return mapper.Map<LoteDto[]>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLote(Lote[] lotes, LoteDto model, int eventoId)
        {
            var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
            model.EventoId = eventoId;
            mapper.Map(model, lote);
            geralPersist.Update<Lote>(lote);

            await geralPersist.SaveChangesAsync();
        }

        private async Task AddLote(int eventoId, LoteDto model)
        {
            var lote = mapper.Map<Lote>(model);
            lote.EventoId = eventoId;

            geralPersist.Add<Lote>(lote);

            await geralPersist.SaveChangesAsync();
        }
    }
}