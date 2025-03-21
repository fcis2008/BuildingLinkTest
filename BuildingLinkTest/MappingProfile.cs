using AutoMapper;
using BuildingLinkTest.DTO;
using BuildingLinkTest.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Driver, DriverDto>().ReverseMap();
        CreateMap<DriverCreateDto, Driver>();
    }
}
