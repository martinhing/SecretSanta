using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SecretSanta.Models
{
    public class SecretSantaContext
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}
