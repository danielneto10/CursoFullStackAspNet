using AutoMapper;
using WebCsharp.Application.Dtos;
using WebCsharp.Domain;

namespace WebCsharp.Application.Helpers
{
    public class WebCsharpProfile : Profile
    {
        public WebCsharpProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
        }
        
    }
}