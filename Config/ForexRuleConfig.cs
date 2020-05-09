using System;
using AutoMapper;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Config
{
    public class ForexRuleConfig:Profile
    {
        public ForexRuleConfig()
        {
            CreateMap<ForexRule, ForexRuleDTO>();
            CreateMap<ForexRuleDTO, ForexRule>();  
        }
    }
}