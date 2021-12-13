using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastRedis.Repository
{
    public interface IRedisRepository
    {
        bool Exists(string key, int db = 0);

        Task<bool> ExistsAsy(string key, int db = 0);


        bool Set<T>(string key, T model, int hours = 24 * 30 * 12, int db = 0);

        bool Set<T>(string key, T model, TimeSpan timeSpan, int db = 0);


        Task<bool> SetAsy<T>(string key, T model, int hours = 24 * 30 * 12, int db = 0);

        Task<bool> SetAsy<T>(string key, T model, TimeSpan timeSpan, int db = 0);


        bool Set(string key, string model, int hours = 24 * 30 * 12, int db = 0);
        bool Set(string key, string model, TimeSpan timeSpan, int db = 0);


        string Get(string key, int db = 0);

        Task<string> GetAsy(string key, int db = 0);


        T Get<T>(string key, int db = 0) where T : class, new();

        Task<T> GetAsy<T>(string key, int db = 0) where T : class, new();


        bool Remove(string key, int db = 0);

        Task<bool> RemoveAsy(string key, int db = 0);


        bool SetDic<T>(Dictionary<string, T> dic, int db = 0);

        Task<bool> SetDicAsy<T>(Dictionary<string, T> dic, int db = 0);


        IDictionary<string, T> GetDic<T>(string[] keys, int db = 0) where T : class, new();

        Task<IDictionary<string, T>> GetDicAsy<T>(string[] keys, int db = 0) where T : class, new();


        bool RemoveDic(string[] keys, int db = 0);

        Task<bool> RemoveDicAsy(string[] keys, int db = 0);


        void Send(string queueName, string message, int db = 0);

        void SendAsy(string queueName, string message, int db = 0);


        string Receive(string queueName, int db = 0);

        Task<string> ReceiveAsy(string queueName, int db = 0);


        void Publish(string channel, string message, int db = 0);

        void PublishAsy(string channel, string message, int db = 0);


        void Receive(string channel, Action<string, string> message, Action<string> subscribe = null, Action<string> unSubscribe = null, int db = 0);

        void ReceiveAsy(string channel, Action<string, string> message, Action<string> subscribe = null, Action<string> unSubscribe = null, int db = 0);
    }
}
