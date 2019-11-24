using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SecretSanta.Models;

namespace SecretSanta.Repository
{
    public class SecretSantaRepository
    {
        private IMongoDatabase db;

        public SecretSantaRepository()
        {
            var client = new MongoClient();
            db = client.GetDatabase("SecretSanta");
        }

        public Guid InsertSecretSantaContext(SecretSantaContext record)
        {
            var collection = db.GetCollection<SecretSantaContext>("SecretSantaContexts");
            collection.InsertOne(record);
            return record.Id;
        }

        public void InsertParticipants(List<Participant> records)
        {
            var collection = db.GetCollection<Participant>("Participants");
            collection.InsertMany(records);
        }

        public void UpdateParticipant(Participant record, Guid id)
        {
            var collection = db.GetCollection<Participant>("Participants");
            collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new UpdateOptions { IsUpsert = true }
            );
        }

        public async Task<List<Participant>> GetAllParticipants(Guid contextId)
        {
            var participants = db.GetCollection<Participant>("Participants");
            return await participants.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Participant> GetParticipant(Guid participantId)
        {
            var collection = db.GetCollection<Participant>("Participants");
            var participants = await collection.Find(new BsonDocument()).ToListAsync();
            return participants.FirstOrDefault(x => x.id == participantId);
        }

        public async Task<Participant> GetParticipantByName(string name)
        {
            var collection = db.GetCollection<Participant>("Participants");
            var participants = await collection.Find(new BsonDocument()).ToListAsync();
            return participants.FirstOrDefault(x => x.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
