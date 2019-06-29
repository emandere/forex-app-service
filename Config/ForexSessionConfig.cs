using System;
using AutoMapper;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Config
{
    public class ForexSessionProfile:Profile
    {
        public ForexSessionProfile()
        {
            CreateMap<ForexSession, ForexSessionMongo>();
            CreateMap<ForexSessionMongo, ForexSession>()
                .ForMember
                ( dest=>dest.StartDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.StartDate).ToString("yyyy-MM-dd")
                        )
                )
                .ForMember
                (dest=>dest.EndDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.EndDate).ToString("yyyy-MM-dd")
                        )
                );

            CreateMap<SessionUser,SessionUserMongo>();
            CreateMap<SessionUserMongo,SessionUser>();

            CreateMap<Accounts,AccountsMongo>();
            CreateMap<AccountsMongo,Accounts>();

            CreateMap<Account,AccountMongo>();
            CreateMap<AccountMongo,Account>();

            CreateMap<BalanceHistory,BalanceHistoryMongo>();
            CreateMap<BalanceHistoryMongo,BalanceHistory>()
                .ForMember
                (
                   dest=>dest.Date, opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.Date).ToString("yyyy-MM-dd")
                        )
                )    
            ;

            CreateMap<Trade,TradeMongo>();
            CreateMap<TradeMongo,Trade>().ForMember(x => x.PL, opt => opt.Ignore());;
            CreateMap<Order,OrderMongo>();
            CreateMap<OrderMongo,Order>();
               
        }

    }
}