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
            CreateMap<ForexSession, ForexSessionDTO>();
            CreateMap<ForexSessionInDTO,ForexSessionDTO>()
                .ForMember(dest => dest.SessionType,
                    opts => opts.MapFrom
                    (
                        src => src.SessionType.Value
                    )
                );
            CreateMap<ForexSessionMongo, ForexSession>()
                .ForMember
                ( dest=>dest.StartDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.StartDate).ToString("yyyy-MM-dd")
                        )
                )
                .ForMember
                ( dest=>dest.CurrentTime,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.CurrentTime).ToString("yyyy-MM-dd")
                        )
                )
                .ForMember
                (dest=>dest.EndDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.EndDate).ToString("yyyy-MM-dd")
                        )
                );
            CreateMap<ForexSessionDTO, ForexSession>();


            CreateMap<SessionUser,SessionUserDTO>();
            CreateMap<SessionUser,SessionUserMongo>();
            CreateMap<SessionUserMongo,SessionUser>();
            CreateMap<SessionUserInDTO,SessionUserDTO>();
            CreateMap<SessionUserDTO,SessionUser>();

            CreateMap<Accounts,AccountsDTO>();
            CreateMap<Accounts,AccountsMongo>();
            CreateMap<AccountsMongo,Accounts>();
            CreateMap<AccountsInDTO,AccountsDTO>();
            CreateMap<AccountsDTO,Accounts>();


            CreateMap<Account,AccountDTO>();
            CreateMap<Account,AccountMongo>();
            CreateMap<AccountMongo,Account>();
            CreateMap<AccountInDTO,AccountDTO>();
            CreateMap<AccountDTO,Account>();
            
            CreateMap<Strategy,StrategyDTO>();
            CreateMap<StrategyMongo,Strategy>();
            CreateMap<Strategy,StrategyMongo>();
            CreateMap<StrategyInDTO,StrategyDTO>();
            CreateMap<StrategyDTO,Strategy>();

            CreateMap<BalanceHistory,BalanceHistoryDTO>();
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
            CreateMap<BalanceHistoryInDTO,BalanceHistoryDTO>();
            CreateMap<BalanceHistoryDTO,BalanceHistory>();

            CreateMap<Trade,TradeDTO>();
            CreateMap<Trade,TradeMongo>();
            CreateMap<TradeMongo,Trade>().ForMember(x => x.PL, opt => opt.Ignore());
            CreateMap<TradeInDTO,TradeDTO>();
            CreateMap<TradeDTO,Trade>();

            CreateMap<Order,OrderDTO>();
            CreateMap<Order,OrderMongo>();
            CreateMap<OrderMongo,Order>();
            CreateMap<OrderInDTO,OrderDTO>();
            CreateMap<OrderDTO,Order>();
               
        }

    }
}