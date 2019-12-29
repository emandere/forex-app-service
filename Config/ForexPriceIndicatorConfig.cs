using System;
using AutoMapper;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Config
{
    public class ForexPriceIndicatorProfile:Profile
    {
        public ForexPriceIndicatorProfile()
        {
            CreateMap<ForexPriceIndicator, ForexPriceIndicatorDTO>();
            CreateMap<ForexPriceIndicator, ForexPriceMongo>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ForexPriceMongo,ForexPriceIndicator>()
                .ForMember(x=>x.Indicator,opt => opt.Ignore());

            CreateMap<DateTime, string>().ConvertUsing(s => s.ToString("MM/dd/yyyy HH:mm:ss"));
        }

    }
}