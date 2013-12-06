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
    public static class AutoMapperConfiguration
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }

        public static void Configure()
        {
            Mapper.CreateMap<Dictionary, TreeListViewModel.Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Childs, opt => opt.Ignore())
                .IgnoreAllNonExisting();

            Mapper.CreateMap<DictionaryItem, TreeListViewModel.Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Childs, opt => opt.Ignore())
                .IgnoreAllNonExisting();

            Mapper.CreateMap<BusinessValueObject, TreeListViewModel.Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent))
                .IgnoreAllNonExisting();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
