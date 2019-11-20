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
        long HkUpdateTicks { get; set; }
    }

    public interface ISessionStore
    {
        Task<List<TOutput>> Read<TInput, TOutput>(string collectionName, string name, TInput value, long contextId = 0);   
        Task Write(string collectionName, ISessionObject sessionObject);
        Task<long> Delete<T>(string collectionName, string user, string name, T value, long contextId = 0);
        Task<long> HasChanges(string collectionName, string user, DateTime since, long contextId = 0);
    }
}
