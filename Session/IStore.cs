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

    public interface IStore
    {
        Task<List<TOutput>> ReadAll<TOutput>(string userName, long contextId = 0);
        Task<List<TOutput>> Read<TInput, TOutput>(string userName, string keyName, TInput value, long contextId = 0);   
        Task Write(ISessionObject sessionObject);
        Task<long> Delete<T>(string user, string name, T value, long contextId = 0);
        Task<long> HasChanges(string user, DateTime since, long contextId = 0);
    }
}
