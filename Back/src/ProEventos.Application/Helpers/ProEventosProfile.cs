using AutoMapper;
using ProEvento.Application.Dtos;
using ProEvento.Domain;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using System;

namespace ProEvento.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
        }
    }
}
