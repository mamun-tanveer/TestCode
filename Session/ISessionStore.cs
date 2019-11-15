﻿using System;
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
        Task<List<TOutput>> Read<TInput, TOutput>(string collectionName, string name, TInput value, long contextId = 0);
        Task<long> UpdateField<T>(string collectionName, string key, string fieldName, T value, long updateTicks = 0);
     }
}
