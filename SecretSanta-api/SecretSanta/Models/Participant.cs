using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace SecretSanta.Models
{
    public class Participant
    {
        [BsonId]
        [JsonIgnore]
        public Guid id { get; set; }
        public Guid contextId { get; set; }
        public string name { get; set; }
        [JsonIgnore]
        public Guid giftRecipient { get; set; }
        public bool isDrawn { get; set; }
        [JsonIgnore]
        public DateTime dateCreated { get; set; }
        public List<string> DisallowedRecipients { get; set; }
    }
}
