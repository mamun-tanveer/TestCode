using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public interface ISessionObject
    {
        object _id { get; set; }
    }

    public interface ISessionStore
    {
        Task Write(string collectionName, ISessionObject sessionObject);
        Task<List<T>> Read<T>(string collectionName, string name, string value, long contextId = 0);
        Task<long> UpdateField<T>(string collectionName, string key, string fieldName, T value, long updateTicks = 0);
     }
}
