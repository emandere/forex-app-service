using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace forex_app_service.Models
{
    public class ForexDailyPriceMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("pair")]
        public string Pair { get; set; }

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("open")]
        public double Open { get; set; }

        [BsonElement("high")]
        public double High { get; set; }

        [BsonElement("low")]
        public double Low { get; set; }

        [BsonElement("close")]
        public double Close { get; set; }

        [BsonElement("datetime")]
        public DateTime Datetime { get; set; }
    }

}
