using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

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

        public async Task<List<TOutput>> ReadAll<TOutput>(string collectionName, string userName, long contextId = 0)
        {
            var collection = db.GetCollection<TOutput>(collectionName);
            var builder = new FilterDefinitionBuilder<TOutput>();

            FilterDefinition<TOutput> query = builder.Eq("User", userName);
            if (collectionName.StartsWith("Context"))
            {
                query &= builder.Eq("ContextId", contextId);
            }

            var cursor = await collection.FindAsync(query);
            return await cursor.ToListAsync();
        }

        public async Task<List<TOutput>> Read<TInput, TOutput>(string collectionName, string userName, string keyName, TInput value, long contextId = 0)
        {
            var collection = db.GetCollection<TOutput>(collectionName);
            var builder = new FilterDefinitionBuilder<TOutput>();

            FilterDefinition<TOutput> query = builder.Eq(keyName, value) & builder.Eq("User", userName);
            if (collectionName.StartsWith("Context"))
            {
                query &= builder.Eq("ContextId", contextId);
            }
               
            var cursor = await collection.FindAsync(query);
            return await cursor.ToListAsync();
        }

        public async Task Write(string collectionName, ISessionObject sessionObject)
        {
            var collection = db.GetCollection<ISessionObject>(collectionName);
            var queryBuilder = new FilterDefinitionBuilder<ISessionObject>();
            if (sessionObject._id == null) sessionObject._id = MongoDB.Bson.ObjectId.GenerateNewId();
            sessionObject.HkUpdateTicks = DateTime.UtcNow.Ticks;
            FilterDefinition<ISessionObject> query = queryBuilder.Eq("_id", sessionObject._id);
            await collection.ReplaceOneAsync(query, sessionObject, new UpdateOptions { IsUpsert = true });
        }

        public async Task<long> Delete<T>(string collectionName, string user, string name, T value, long contextId = 0) 
        {
            var collection = db.GetCollection<ISessionObject>(collectionName);
            var queryBuilder = new FilterDefinitionBuilder<ISessionObject>();
            var queryList = new List<FilterDefinition<ISessionObject>>();
            queryList.Add(queryBuilder.Eq("User", user));                
            queryList.Add(queryBuilder.Eq("ContextId", contextId));
            if(string.IsNullOrEmpty(name) == false) queryList.Add(queryBuilder.Eq(name, value));
            var result = await collection.DeleteManyAsync(queryBuilder.And(queryList));
            return result.DeletedCount;
        }

        public async Task<long> HasChanges(string collectionName, string user, DateTime since, long contextId = 0)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var builder = new FilterDefinitionBuilder<BsonDocument>();
            var query = builder.And(builder.Eq("User", user), builder.Gt("HkUpdateTicks", since.Ticks));
            if (collectionName.StartsWith("Context"))
            {

                FilterDefinition<BsonDocument> contextQuery = builder.Eq("ContextId", contextId);
                query = builder.And(query, contextQuery);
            }

            var results  = await collection.Find(query).Project("{ HkUpdateTicks: 1}").ToListAsync();
 
            long lastUpdateTicks = 0;
            foreach(var result in results)
            {
                long hkUpdateTicks = result["HkUpdateTicks"].ToInt64();
                lastUpdateTicks = (hkUpdateTicks > lastUpdateTicks) ? hkUpdateTicks : lastUpdateTicks;
            }
                
            return lastUpdateTicks;
        }

     
    }
}
