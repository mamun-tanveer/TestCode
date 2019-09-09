using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace QueryLoop
{
    class MongoDB
    {

        static MongoClient client;
        private IMongoDatabase db;
        private IMongoCollection<BsonDocument> collection;

        public MongoDB(string connectionString, string dbName, string collectionName)
        {
            var settings = new MongoClientSettings();
            if (client == null) client = new MongoClient(connectionString);
            db = client.GetDatabase(dbName);
            collection = db.GetCollection<BsonDocument>(collectionName);
        }

        public async Task<IEnumerable<Result>> DoQuery(string queryJson, string projectionJson)
        {

            var returnList = new List<Result>();

            var filter = new JsonFilterDefinition<BsonDocument>(queryJson);

            var docs = await collection.Find(filter).Project(projectionJson).ToListAsync();


            foreach (BsonDocument resultDoc in docs)
            {
                var result = new Result();
                foreach(BsonElement element in resultDoc)
                {
                    result.Names.Add(element.Name);
                    result.Values.Add(element.Value.ToString());  
                }
                returnList.Add(result);  
            }

            return returnList;
        }
    }
}
