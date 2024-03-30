using System;
using MongoDB.Bson.Serialization.Attributes;

namespace src.Infra.Collection;

public class PaymentCollection
{
    [BsonElement]
    public int? Period { get; set; }

    [BsonElement]
    public int Amount { get; set; }

    [BsonElement]
    public DateTime Date { get; set; }
}
