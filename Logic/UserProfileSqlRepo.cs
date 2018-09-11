using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace SimpleBot.Logic
{
    public class UserProfileSqlRepo : IUserProfileRepository
    {
        private IMongoCollection<UserProfileMongo> _collection;
        private IDbConnection db;

        public UserProfileSqlRepo(string connectionString)
        {
             db = new SqlConnection(connectionString);

        }

        public UserProfile GetProfile(string id)
        {
            return db.Query<UserProfile>("select * from UserProfile where Id = @id", new { id }).FirstOrDefault();
            
            
            
                


        }

        public void SetProfile(string id, UserProfile profile)
        {
            UserProfile p = GetProfile(id);
            if (p!=null)
            {
                db.Execute("update UserProfile set Visitas=@Visitas where Id=@id", new { id = id, Visitas = p.Visitas});
            }
            else
            {
                db.Execute("insert into UserProfile (Id,Visitas) values (@id,@Visitas)", new { id=id, Visitas= profile.Visitas });
            }
        }
    }
}