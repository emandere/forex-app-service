using System;
namespace forex_app_service.Domain
{
    public  class ForexPrice
    {
        public string Instrument { get; set; }
        public DateTime Time { get; set; }   
        public double Bid { get; set; }
        public double Ask { get; set; }
    }   
}