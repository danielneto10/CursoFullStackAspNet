using System;
using System.Threading.Tasks;
using WebCsharp.Application.Contratos;
using WebCsharp.Domain;
using WebCsharp.Persistence.Contratos;

namespace WebCsharp.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist eventoPersist;
        private readonly IGeralPersist geralPersist;
        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist)
        {
            this.geralPersist = geralPersist;
            this.eventoPersist = eventoPersist;

        }

        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                geralPersist.Add<Evento>(model);
                if(await geralPersist.SaveChangesAsync())
                {
                    return await eventoPersist.GetEventoByIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await eventoPersist.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return null;

                model.Id = evento.Id;

                geralPersist.Update(model);
                if(await geralPersist.SaveChangesAsync())
                {
                    return await eventoPersist.GetEventoByIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await eventoPersist.GetEventoByIdAsync(eventoId, false);
                if(evento == null) throw new Exception("Evento para delete não foi encontrado!");

                geralPersist.Delete<Evento>(evento);
                return await geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoPersist.GetAllEventosAsync(includePalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
                if(evento == null) return null;

                return evento;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

    }
}