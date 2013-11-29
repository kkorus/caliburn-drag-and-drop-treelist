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
            Mapper.CreateMap<Dictionary, TreeListViewModel.Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Childs, opt => opt.Ignore());

            Mapper.CreateMap<DictionaryItem, TreeListViewModel.Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Childs, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}
