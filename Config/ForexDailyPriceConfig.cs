using System;
using AutoMapper;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Config
{
    public class ForexDailyPriceProfile:Profile
    {
        public ForexDailyPriceProfile()
        {
            
             CreateMap<ForexDailyPriceDTO, ForexDailyPriceMongo>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ForexDailyPriceMongo, ForexDailyPriceDTO>();
            CreateMap<ForexDailyPriceMongo, ForexDailyPrice>();
            CreateMap<DateTime, string>().ConvertUsing(s => s.ToString("MM/dd/yyyy HH:mm:ss"));
        }

    }
}