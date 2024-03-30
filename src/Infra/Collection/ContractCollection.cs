using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace src.Infra.Collection;

public class ContractCollection
{
    [BsonId]
    public ObjectId Id { get; set; } 

    [BsonElement]
    public string Description { get; set; }

    [BsonElement]
    public double Amount { get; set; }

    [BsonElement]
    public int Periods { get; set; }

    [BsonElement]
    public DateTime Date { get; set; }

    [BsonElement]
    public List<PaymentCollection> Payments { get; set; }    
}
