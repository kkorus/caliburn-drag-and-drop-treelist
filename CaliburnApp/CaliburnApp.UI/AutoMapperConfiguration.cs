using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CaliburnApp.Domain.Entities;
using CaliburnApp.UI.ViewModels;

namespace CaliburnApp.UI
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Dictionary, Node>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            Mapper.AssertConfigurationIsValid();
        }
    }
}
