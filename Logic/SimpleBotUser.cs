using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using MongoDB.Driver;
using MongoDB.Bson;
using static MongoDB.Bson.Serialization.BsonDeserializationContext;

namespace SimpleBot
{
    public class SimpleBotUser
    {

        public static string Reply(Message message)
        {
            var id = message.Id;
            var profile = GetProfile(id);

            profile.Visitas++;
            SetProfile(id, profile);


            return $"{message.User} disse '{message.Text} e mandou {profile.Visitas} mensagens'";
        }

        public static void SalvarHistorico(Message message)
        {
            MongoClient a = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = a.GetDatabase("db05");
            var col = db.GetCollection<BsonDocument>("tabela01");
            var doc = new BsonDocument()
            {
                {"texto",message.Text},
                {"id",message.Id},
                {"datetime",DateTime.Now},
            };
            col.InsertOne(doc);

            var filtro = Builders<BsonDocument>.Filter.Eq("id", message.Id);            if (col.Find(filtro) == null)
            {

            }

        }

        public static UserProfile GetProfile(string id)
        {
            MongoClient a = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = a.GetDatabase("db05");
            var col = db.GetCollection<BsonDocument>("tabela01");

            var filtro = Builders<BsonDocument>.Filter.Eq("id",id);
            var profile = col.Find(filtro);
            
            return new UserProfile { Id=id, Visitas = 1};
        }

        public static void SetProfile(string id, UserProfile profile)
        {
            MongoClient a = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = a.GetDatabase("db05");
            var col = db.GetCollection<BsonDocument>("tabela01");
            var filtro = Builders<BsonDocument>.Filter.Eq("id", id);
            col.ReplaceOne(filtro, new BsonDocument() {{ "id",id },{"visitas",profile.Visitas}});

            
            
            
        }
    }
}