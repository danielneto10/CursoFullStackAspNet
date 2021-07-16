using System;
using System.Threading.Tasks;
using AutoMapper;
using WebCsharp.Application.Contratos;
using WebCsharp.Application.Dtos;
using WebCsharp.Domain;
using WebCsharp.Persistence.Contratos;

namespace WebCsharp.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist eventoPersist;
        private readonly IGeralPersist geralPersist;
        private readonly IMapper mapper;
        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            this.mapper = mapper;
            this.geralPersist = geralPersist;
            this.eventoPersist = eventoPersist;

        }

        public async Task<EventoDto> AddEvento(EventoDto model)
        {
            try
            {
                var evento = mapper.Map<Evento>(model);

                geralPersist.Add<Evento>(evento);
                if (await geralPersist.SaveChangesAsync())
                {
                    var retorno = await eventoPersist.GetEventoByIdAsync(evento.Id, false);
                    return mapper.Map<EventoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await eventoPersist.GetEventoByIdAsync(eventoId, false);
                if (evento == null) return null;
                model.Id = evento.Id;
                
                mapper.Map(model, evento);

                geralPersist.Update<Evento>(evento);
                if (await geralPersist.SaveChangesAsync())
                {
                    var retorno = await eventoPersist.GetEventoByIdAsync(evento.Id, false);
                    return mapper.Map<EventoDto>(retorno);
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
                if (evento == null) throw new Exception("Evento para delete não foi encontrado!");

                geralPersist.Delete<Evento>(evento);
                return await geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoPersist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                var eventoDto = mapper.Map<EventoDto[]>(eventos);

                return eventoDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                var eventoDto = mapper.Map<EventoDto[]>(eventos);

                return eventoDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) return null;

                var eventoDto = mapper.Map<EventoDto>(evento);

                return eventoDto;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}