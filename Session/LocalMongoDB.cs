using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Session
{
    internal class LocalMongoDB : ISessionStore
    {

        private IMongoDatabase db; 
       
        public LocalMongoDB()
        {
            var client = new MongoClient(@"mongodb://localhost/");
            db = client.GetDatabase("Session");
        }

        public async Task<List<T>> Read<T>(string collectionName, string name, string value, long contextId = 0)
        {
            var collection = db.GetCollection<T>(collectionName);
            FilterDefinition<T> query = "{" + name + ":" + value + "}";
            if (contextId > 0)
            {
                var builder = new FilterDefinitionBuilder<T>();
                FilterDefinition<T> contextQuery = "{ContextId" + ":" + contextId.ToString() + "}";
                query = builder.And(query, contextQuery);         
            }
            var cursor = await collection.FindAsync(query);
            return await cursor.ToListAsync();
        }

        public Task<long> UpdateField<T>(string collectionName, string key, string fieldName, T value, long updateTicks = 0)
        {
            throw new NotImplementedException();
        }

        public async Task Write(string collectionName, ISessionObject sessionObject)
        {
            var collection = db.GetCollection<ISessionObject>(collectionName);
            var queryBuilder = new FilterDefinitionBuilder<ISessionObject>();
            if (sessionObject._id == null) sessionObject._id = MongoDB.Bson.ObjectId.GenerateNewId();
            FilterDefinition<ISessionObject> query = queryBuilder.Eq("_id", sessionObject._id);
            await collection.ReplaceOneAsync(query, sessionObject, new UpdateOptions { IsUpsert = true });
        }
    }
}
