using AutoMapper;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Entities;

namespace BlogApi.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Article, ArticleDTO>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));
    }
}