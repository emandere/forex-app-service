using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Config
{
    public class ForexPriceProfile:Profile
    {
         static Dictionary<string,string> pairToInstrument= new Dictionary<string,string>()
        {
            {"AUD_USD","AUDUSD"},
            {"EUR_USD","EURUSD"},
            {"GBP_USD","GBPUSD"},
            {"NZD_USD","NZDUSD"},
            {"USD_CAD","USDCAD"},
            {"USD_CHF","USDCHF"},
            {"USD_JPY","USDJPY"},
        };
        public ForexPriceProfile()
        {
            CreateMap<ForexPrice, ForexPriceDTO>();
            CreateMap<ForexPrice, ForexPriceMongo>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ForexPriceDTO, ForexPriceMongo>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.UTCTime))
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ForexPriceDTO, ForexRealPriceMongo>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ForexPriceMongo, ForexPrice>();
            CreateMap<ForexPriceMongo, ForexPriceDTO>();
            CreateMap<ForexRealPriceMongo, ForexPriceDTO>();
            CreateMap<ForexQuotesDTO,ForexPriceDTO>()
                .ForMember(dest => dest.Instrument, opt => opt.MapFrom( src => pairToInstrument[src.Instrument]))
                .ForMember(dest => dest.Time, opt => opt.MapFrom( src => src.Candles.Last().Time.Split('.',StringSplitOptions.None)[0] + "Z"))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom( src => src.Candles.Last().Bid.C))
                .ForMember(dest => dest.Ask, opt => opt.MapFrom( src => src.Candles.Last().Ask.C));
            CreateMap<DateTime, string>().ConvertUsing(s => s.ToString("MM/dd/yyyy HH:mm:ss"));
        }

    }
}