using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace forex_app_service.Domain
{
    public  class ForexSession
    {
        public string Id { get; set; }
        public string idinfo { get; set; }

        public string SessionType { get; set; }

        public string ExperimentId { get; set; }

        public string StartDate { get; set; }

       
        public string EndDate { get; set; }

        
        public string LastUpdatedTime { get; set; }

        
        public string CurrentTime { get; set; }

        public Strategy Strategy { get; set; }

        public SessionUser SessionUser { get; set; }

        public string PercentComplete { get; set; }

        public string elapsedTime { get; set; }
        public string beginSessionTime { get; set; }
        public string endSessionTime { get; set; }
    }

    public  class SessionUser
    {
        
        public string Id { get; set; }

        public string idinfo { get; set; }

        public object Status { get; set; }
        public Accounts Accounts { get; set; }
    }

    public  class Accounts
    {
        
        public Account Primary { get; set; }

        public Account Secondary { get; set; }
    }

    public  class Account
    {
       
        public string Id { get; set; }

        public string idinfo { get; set; }

        public double Cash { get; set; }

        public long Margin { get; set; }

       
        public long MarginRatio { get; set; }

      
        public double RealizedPl { get; set; }

      
        public Trade[] Trades { get; set; }

      
        public Order[] Orders { get; set; }

      
        public Trade[] ClosedTrades { get; set; }

     
        public BalanceHistory[] BalanceHistory { get; set; }

     
        public long Idcount { get; set; }
    }

    public  class BalanceHistory
    {
     
        public string Date { get; set; }

     
        public double Amount { get; set; }
    }

    public  class Trade
    {
        private double _pl;
        public long? Id { get; set; }

     
        public long? idinfo { get; set; }

     
        public string Pair { get; set; }

       
        public long Units { get; set; }

      
        public string OpenDate { get; set; }

      
        public string CloseDate { get; set; }

     
        public bool Long { get; set; }

       
        public double OpenPrice { get; set; }

      
        public double ClosePrice { get; set; }

        public double? StopLoss { get; set; }

        public double? TakeProfit { get; set; }


        public bool Init { get; set; }

        public double PL { get => PLCalc();}

        double Position()
        {
            if(Long)
                return 1.0;
            else
                return -1.0;
        }

        double adj()
        {
            double adj= (Pair=="USDJPY") ? 0.01 : 1.0;
            return adj;
        }
        public double PLCalc()
        {
            return Units * Position() * (ClosePrice - OpenPrice)*adj();
        }

        
    }

    public  class Order
    {
        public object ExpirationDate { get; set; }

        public double Triggerprice { get; set; }

        public bool Expired { get; set; }

        public bool Above { get; set; }

        public Trade Trade { get; set; }
    }

}
