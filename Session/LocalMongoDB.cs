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

        public async Task<List<TOutput>> Read<TInput, TOutput>(string collectionName, string name, TInput value, long contextId = 0)
        {
            var collection = db.GetCollection<TOutput>(collectionName);
            var builder = new FilterDefinitionBuilder<TOutput>();

            FilterDefinition<TOutput> query = builder.Eq(name, value);
            if (contextId > 0)
            {

                FilterDefinition<TOutput> contextQuery = builder.Eq("ContextId", value);
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
