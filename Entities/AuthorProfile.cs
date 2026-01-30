using AutoMapper;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<AddAuthorDto, Author>();
        CreateMap<UpdateAuthorDto, Author>();
        CreateMap<Author, UpdateAuthorDto>();
    }
}
