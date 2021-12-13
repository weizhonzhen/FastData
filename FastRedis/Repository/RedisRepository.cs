﻿using FastRedis.Config;
using NServiceKit.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FastRedis.Repository
{
    public class RedisRepository : IRedisRepository
    {
        private int db = 0;
        private IRedisClient Context;
        public RedisRepository()
        {
            var config = RedisConfig.GetConfig();

            Context = new PooledRedisClientManager(config.WriteServerList.Split(',')
             , config.ReadServerList.Split(','), new RedisClientManagerConfig
             {
                 DefaultDb = db,
                 MaxReadPoolSize = config.MaxReadPoolSize,
                 MaxWritePoolSize = config.MaxWritePoolSize,
                 AutoStart = config.AutoStart
             }).GetClient();
        }

        #region 是否存在
        /// <summary>
        /// 是否存在 
        /// </summary>
        /// <returns></returns>
        public bool Exists(string key, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return false;
                else
                    return Context.ContainsKey(key);
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Exists", key);
                return false;
            }
        }
        #endregion

        #region 是否存在 asy
        /// <summary>
        /// 是否存在 asy
        /// </summary>
        /// <returns></returns>
        public Task<bool> ExistsAsy(string key, int db = 0)
        {
            return Task.Run(() =>
           {
               return Exists(key, db);
           });
        }
        #endregion


        #region 设置值 item
        /// <summary>
        /// 设置值 item
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public bool Set<T>(string key, T model, int hours = 24 * 30 * 12, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return false;
                else
                    return Context.Set<T>(key, model, DateTime.Now.AddHours(hours));
            }
            catch (RedisException ex)
            {
                SaveLog<T>(ex, "Set<T>");
                return false;
            }
        }
        #endregion

        #region 设置值 item
        /// <summary>
        /// 设置值 item
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public bool Set<T>(string key, T model, TimeSpan timeSpan, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return false;
                else
                    return Context.Set<T>(key, model, timeSpan);
            }
            catch (RedisException ex)
            {
                SaveLog<T>(ex, "Set<T>");
                return false;
            }
        }
        #endregion


        #region 设置值 item asy
        /// <summary>
        /// 设置值 item asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public Task<bool> SetAsy<T>(string key, T model, int hours = 24 * 30 * 12, int db = 0)
        {
            return Task.Run(() =>
           {
               return Set<T>(key, model, hours, db);
           });
        }
        #endregion

        #region 设置值 item asy
        /// <summary>
        /// 设置值 item asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public Task<bool> SetAsy<T>(string key, T model, TimeSpan timeSpan, int db = 0)
        {
            return Task.Run(() =>
            {
                return Set<T>(key, model, timeSpan, db);
            });
        }
        #endregion


        #region 设置值 item
        /// <summary>
        /// 设置值 item
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public bool Set(string key, string model, int hours = 24 * 30 * 12, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return false;
                else
                    return Context.Set<string>(key, model, DateTime.Now.AddHours(hours));

            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Set", key);
                return false;
            }
        }
        #endregion


        #region 设置值 item
        /// <summary>
        /// 设置值 item
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public bool Set(string key, string model, TimeSpan timeSpan, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return false;
                else
                    return Context.Set<string>(key, model, timeSpan);

            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Set", key);
                return false;
            }
        }
        #endregion


        #region 设置值 item asy
        /// <summary>
        /// 设置值 item asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public Task<bool> SetAsy(string key, string model, int hours = 24 * 30 * 12, int db = 0)
        {
            return Task.Run(() =>
           {
               return Set(key, model, hours, db);
           });
        }
        #endregion

        #region 设置值 item asy
        /// <summary>
        /// 设置值 item asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="model">值</param>
        /// <param name="hours">存期限</param>
        /// <returns></returns>
        public Task<bool> SetAsy(string key, string model, TimeSpan timeSpan, int db = 0)
        {
            return Task.Run(() =>
            {
                return Set(key, model, timeSpan, db);
            });
        }
        #endregion


        #region 获取值 item
        /// <summary>
        /// 获取值 item
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string Get(string key, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return "";
                else
                    return Context.Get<string>(key);
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Get", key);
                return "";
            }
        }
        #endregion

        #region 获取值 item asy
        /// <summary>
        /// 获取值 item asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<string> GetAsy(string key, int db = 0)
        {
            return Task.Run(() =>
           {
               return Get(key, db);
           });
        }
        #endregion


        #region 获取值 item
        /// <summary>
        /// 获取值 item
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key, int db = 0) where T : class, new()
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return new T();
                else
                    return Context.Get<T>(key);
            }
            catch (RedisException ex)
            {
                SaveLog<T>(ex, "Get<T>");
                return new T();
            }
        }
        #endregion

        #region 获取值 item asy
        /// <summary>
        /// 获取值 item asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<T> GetAsy<T>(string key, int db = 0) where T : class, new()
        {
            return Task.Run(() =>
           {
               return Get<T>(key, db);
           });
        }
        #endregion


        #region 删除值 item
        /// <summary>
        /// 删除值 item
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(key))
                    return false;
                else
                    return Context.Remove(key);
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "RemoveItem", key);
                return false;
            }
        }
        #endregion

        #region 删除值 item asy
        /// <summary>
        /// 删除值 item asy
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<bool> RemoveAsy(string key, int db = 0)
        {
            return Task.Run(() =>
           {
               return Remove(key, db);
           });
        }
        #endregion


        #region 设置值 Dic
        /// <summary>
        /// 设置值 Dic
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dic">字典</param>
        /// <returns></returns>
        public bool SetDic<T>(Dictionary<string, T> dic, int db = 0)
        {
            try
            {
                Context.Db = db;
                Context.SetAll<T>(dic);
                return true;
            }
            catch (RedisException ex)
            {
                SaveLog<T>(ex, "SetDic<T>");
                return false;
            }
        }
        #endregion

        #region 设置值 Dic Asy
        /// <summary>
        /// 设置值 Dic Asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dic">字典</param>
        /// <returns></returns>
        public Task<bool> SetDicAsy<T>(Dictionary<string, T> dic, int db = 0)
        {
            return Task.Run(() =>
           {
               return SetDic<T>(dic, db);
           });
        }
        #endregion


        #region 获取值 dic
        /// <summary>
        /// 获取值 dic
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public IDictionary<string, T> GetDic<T>(string[] keys, int db = 0) where T : class, new()
        {
            try
            {
                Context.Db = db;
                return Context.GetAll<T>(keys);
            }
            catch (RedisException ex)
            {
                SaveLog<T>(ex, "GetDic<T>");
                return new Dictionary<string, T>();
            }
        }
        #endregion

        #region 获取值 dic asy
        /// <summary>
        /// 获取值 dic asy
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public Task<IDictionary<string, T>> GetDicAsy<T>(string[] keys, int db = 0) where T : class, new()
        {
            return Task.Run(() =>
           {
               return GetDic<T>(keys, db);
           });
        }
        #endregion


        #region 删除值 dic
        /// <summary>
        /// 删除值 dic
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public bool RemoveDic(string[] keys, int db = 0)
        {
            try
            {
                Context.Db = db;
                Context.RemoveAll(keys);

                return true;
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "RemoveDic", keys.ToString());
                return false;
            }
        }
        #endregion

        #region 删除值 dic asy
        /// <summary>
        /// 删除值 dic asy
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public Task<bool> RemoveDicAsy(string[] keys, int db = 0)
        {
            return Task.Run(() =>
           {
               return RemoveDic(keys, db);
           });
        }
        #endregion


        #region 发布消息(生产者消费者模式)
        /// <summary>
        /// 发布消息(生产者消费者模式)
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息</param>
        /// <param name="db"></param>
        public void Send(string queueName, string message, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(queueName))
                    return;
                else
                    Context.EnqueueItemOnList(queueName, message);
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Send", "");
            }
        }
        #endregion

        #region 发布消息(生产者消费者模式) asy
        /// <summary>
        /// 发布消息(生产者消费者模式) asy
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息</param>
        /// <param name="db"></param>
        public void SendAsy(string queueName, string message, int db = 0)
        {
            Task.Run(() =>
            {
                Send(queueName, message, db);
            });
        }
        #endregion

        #region 接收消息(生产者消费者模式)
        /// <summary>
        /// 接收消息(生产者消费者模式)
        /// </summary>
        public string Receive(string queueName, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(queueName))
                    return "";
                else
                    return Context.DequeueItemFromList(queueName);
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Receive", "");
                return "";
            }
        }
        #endregion

        #region 接收消息(生产者消费者模式) asy
        /// <summary>
        /// 接收消息(生产者消费者模式) asy
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息</param>
        /// <param name="db"></param>
        public Task<string> ReceiveAsy(string queueName, int db = 0)
        {
            return Task.Run(() =>
           {
               return Receive(queueName, db);
           });
        }
        #endregion


        #region 发布消息(发布者订阅者模式)
        /// <summary>
        /// 发布消息(发布者订阅者模式)
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="message">消息</param>
        /// <param name="db"></param>
        public void Publish(string channel, string message, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(channel))
                    return;
                else
                    Context.PublishMessage(channel, message);
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Publish", "");
            }
        }
        #endregion

        #region 发布消息(发布者订阅者模式) asy
        /// <summary>
        /// 发布消息(发布者订阅者模式) asy
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="message">消息</param>
        /// <param name="db"></param>
        public void PublishAsy(string channel, string message, int db = 0)
        {
            Task.Run(() =>
            {
                Publish(channel, message, db);
            });
        }
        #endregion

        #region 接收消息(发布者订阅者模式)
        /// <summary>
        /// 接收消息(发布者订阅者模式)
        /// </summary>
        /// <param name="message">接收</param>
        /// <param name="subscribe">订阅</param>
        /// <param name="unSubscribe">取消订阅</param>
        /// <param name="channel">频道</param>
        /// <param name="db"></param>
        public void Receive(string channel, Action<string, string> message, Action<string> subscribe = null, Action<string> unSubscribe = null, int db = 0)
        {
            try
            {
                Context.Db = db;
                if (string.IsNullOrEmpty(channel))
                    return;
                else
                {
                    using (var item = Context.CreateSubscription())
                    {
                        item.OnMessage = message;
                        item.OnSubscribe = subscribe;
                        item.OnUnSubscribe = unSubscribe;
                        item.SubscribeToChannels(channel);
                    }
                }
            }
            catch (RedisException ex)
            {
                SaveLog(ex, "Receive", "");
            }
        }
        #endregion

        #region 接收消息(发布者订阅者模式) asy
        /// <summary>
        /// 接收消息(发布者订阅者模式) asy
        /// </summary>
        public void ReceiveAsy(string channel, Action<string, string> message, Action<string> subscribe = null, Action<string> unSubscribe = null, int db = 0)
        {
            Task.Run(() =>
            {
                Receive(channel, message, subscribe, unSubscribe, db);
            }).ConfigureAwait(false);
        }
        #endregion


        #region 出错日志
        /// <summary>
        /// 出错日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <param name="CurrentMethod"></param>
        private void SaveLog<T>(Exception ex, string CurrentMethod)
        {
            SaveLog(string.Format("方法：{0},对象：{1},出错详情：{2}", CurrentMethod, typeof(T).Name, ex.ToString()), "redis_exp");
        }
        #endregion

        #region 出错日志
        /// <summary>
        /// 出错日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <param name="CurrentMethod"></param>
        private void SaveLog(Exception ex, string CurrentMethod, string key)
        {
            SaveLog(string.Format("方法：{0},键：{1},出错详情：{2}", CurrentMethod, key, ex.ToString()), "redis_exp");
        }
        #endregion

        #region 写日志
        /// <summary>
        /// 说明：写日记
        /// </summary>
        /// <param name="StrContent">日志内容</param>
        private void SaveLog(string logContent, string fileName, string headName = "", bool IsWrap = false, int logCount = 10)
        {
            var path = string.Format("{0}/App_Data/log/{1}/{2}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy-MM"), DateTime.Now.ToString("yyyy-MM-dd"));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (fileName == "")
                fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd-HH"));
            else
                fileName = string.Format("{0}_{1}.txt", fileName, DateTime.Now.ToString("yyyy-MM-dd-HH"));

            path = string.Format("{0}/{1}", path, fileName);

            if (!File.Exists(path))
                using (var fs = File.Create(path)) { }

            //写日志
            using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var m_streamWriter = new StreamWriter(fs))
                {
                    m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    m_streamWriter.WriteLine(string.Format("[{0}]{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logContent));
                    m_streamWriter.WriteLine("");
                    m_streamWriter.Flush();
                    m_streamWriter.Close();
                    fs.Close();
                }
            }
        }
        #endregion
    }
}
