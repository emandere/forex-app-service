using System;
namespace forex_app_service.Domain
{
    public  class ForexPriceIndicator
    {
        public string Instrument { get; set; }
        public string Time { get; set; }   
        public double Bid { get; set; }
        public double Ask { get; set; }
        public string Indicator { get; set; }
    }   
}