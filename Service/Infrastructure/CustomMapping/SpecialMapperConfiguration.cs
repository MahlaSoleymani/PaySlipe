using AutoMapper;
using Common.Utilities;
using Entities.Infrastructure;
using Newtonsoft.Json;

namespace Service.Infrastructure.CustomMapping
{
    public static class SpecialMapperConfiguration
    {
        public static void AddSpecialMapperProfile(this IMapperConfigurationExpression config)
        {

            #region Convert

            // config.CreateJsonMaps();

           config.CreateMap<Dictionary<string, string>, string>().ConvertUsing(new AutoMapperTypeConvertors.DictionaryToStringTypeConvertor());
            config.CreateMap<string, Dictionary<string, string>>().ConvertUsing(new AutoMapperTypeConvertors.StringToDictionaryTypeConvertor());

            config.CreateMap<DateTime, string>().ConvertUsing(new AutoMapperTypeConvertors.DateToStringTypeConvertor());
            config.CreateMap<DateOnly, string>().ConvertUsing(new AutoMapperTypeConvertors.DateOnlyToStringTypeConvertor());

            config.CreateMap<string, DateTime>().ConvertUsing(new AutoMapperTypeConvertors.StringToDateTypeConvertor());

            config.CreateMap<DateTime?, string>().ConvertUsing(new AutoMapperTypeConvertors.DateNullableToStringTypeConvertor());
            config.CreateMap<string, DateTime?>().ConvertUsing(new AutoMapperTypeConvertors.StringToDateNullableTypeConvertor());

            config.CreateMap<TimeSpan, string>().ConvertUsing(new AutoMapperTypeConvertors.TimeSpanToStringTypeConvertor());
            config.CreateMap<string, TimeSpan>().ConvertUsing(new AutoMapperTypeConvertors.StringToTimeSpanTypeConvertor());

            config.CreateMap<IList<int>, int>().ConvertUsing(new AutoMapperTypeConvertors.BinariesToIntegerConvertor());
            config.CreateMap<int, IList<int>>().ConvertUsing(new AutoMapperTypeConvertors.IntegerToBinariesConvertor());

            // config.CreateMap<IEnumerable<Option>, OptionDto>().ConvertUsing(new OptionToDtoConvertor<Option, OptionDto>());
            // config.CreateMap<OptionDto, IEnumerable<Option>>().ConvertUsing(new DtoToOptionConvertor<OptionDto, Option>());

           
            #endregion

        }

        public static void CreateJsonMaps(this IMapperConfigurationExpression config)
        {
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsAbstract && !p.IsInterface && p.GetInterface(typeof(IJsonProperty<>).Name) != null)
                .ToList();

            foreach (var type in list.Select(x => x.GetInterfaces()[0].GetGenericArguments()[0]))
            {
                config.CreateMap(typeof(string), type).ConvertUsing(x => JsonConvert.DeserializeObject(x.ToString(), type));
                config.CreateMap(type, typeof(string)).ConvertUsing(x => JsonConvert.SerializeObject(x));
            }
        }
    }
}