using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleBot.Logic
{
    public class UserProfileMongoRepo : IUserProfileRepository
    {
        private IMongoCollection<UserProfileMongo> _collection;

        public UserProfileMongoRepo(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("DB01");
            var collection = db.GetCollection<UserProfileMongo>("profile");

            this._collection = collection;
        }

        public UserProfile GetProfile(string id)
        {
            var filter = Builders<UserProfileMongo>.Filter.Eq("_id", id);
            
            var cursor = _collection.Find(filter);

            var profile = cursor.FirstOrDefault();

            return new UserProfile
            {
                Id = profile._id,
                Visitas = profile.Visitas
            };
        }

        public void SetProfile(string id, UserProfile profile)
        {
            var filter = Builders<UserProfileMongo>.Filter.Eq("_id", id);

            var doc = new UserProfileMongo
            {
                _id = profile.Id,
                Visitas = profile.Visitas
            };

            _collection.ReplaceOne(filter, doc, new UpdateOptions { IsUpsert = true });
        }
    }
}