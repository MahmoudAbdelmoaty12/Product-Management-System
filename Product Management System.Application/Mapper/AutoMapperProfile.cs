using AutoMapper;
using Microsoft.AspNetCore.Http;
using Product_Management_System.Application.Dtos;
using Product_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_System.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {


            CreateMap<CreateOrUpdateProductDtos, Product>().ReverseMap();
            CreateMap<GetAllProductsDtos, Product>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<IFormFile, Image>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName)).ReverseMap();
        }
    }
}
