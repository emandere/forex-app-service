using AutoMapper;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Config
{
    public class ForexPriceProfile:Profile
    {
        public ForexPriceProfile()
        {
            CreateMap<ForexPrice, ForexPriceMongo>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

    }
}