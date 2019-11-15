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

                FilterDefinition<TOutput> contextQuery = builder.Eq("ContextId", contextId);
                query = builder.And(query, contextQuery);         
            }
            var cursor = await collection.FindAsync(query);
            return await cursor.ToListAsync();
        }

        public async Task Write(string collectionName, ISessionObject sessionObject)
        {
            var collection = db.GetCollection<ISessionObject>(collectionName);
            var queryBuilder = new FilterDefinitionBuilder<ISessionObject>();
            if (sessionObject._id == null) sessionObject._id = MongoDB.Bson.ObjectId.GenerateNewId();
            sessionObject.HkUpdateTicks = DateTime.Now.Ticks;
            FilterDefinition<ISessionObject> query = queryBuilder.Eq("_id", sessionObject._id);
            await collection.ReplaceOneAsync(query, sessionObject, new UpdateOptions { IsUpsert = true });
        }

        public async Task<long> Delete<T>(string collectionName, string user, string name, T value, long contextId) 
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
    }
}
