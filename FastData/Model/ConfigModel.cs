﻿using System;

namespace FastData.Model
{
    /// <summary>
    /// 配置连接实体
    /// </summary>
    [Serializable]
    public sealed class ConfigModel
    {
        /// <summary>
        /// 数据库连接DLL名
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnStr { get; set; }

        /// <summary>
        /// 是否输出SQL
        /// </summary>
        public bool IsOutSql { get; set; }

        /// <summary>
        /// 是否输入错误
        /// </summary>
        public bool IsOutError { get; set; }

        /// <summary>
        /// 是否缓存属性
        /// </summary>
        public bool IsPropertyCache { get; set; }
        
        /// <summary>
        /// 参数串字符
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 连接数
        /// </summary>
        public int LinkCount { get; set; }

        /// <summary>
        /// dblink
        /// </summary>
        public string DbLinkName { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// map文件是否放数据库
        /// </summary>
        public bool IsMapSave { get; set; }

        /// <summary>
        /// map文件是否加密
        /// </summary>
        public bool IsEncrypt { get; set; }
                
        /// <summary>
        /// 设计模式
        /// </summary>
        public string DesignModel { get; set; }

        /// <summary>
        /// sql存放类型file,db 
        /// </summary>
        public string SqlErrorType { get; set; }

        /// <summary>
        /// 缓存类型 web,redis
        /// </summary>
        public string CacheType { get; set; }

        /// <summary>
        /// 是否更新缓存
        /// </summary>
        public bool IsUpdateCache { get; set; }

        /// <summary>
        /// 切换数据库
        /// </summary>
        public bool IsChangeDb { get; set; }

        public string FactoryClient { get; set; }
    }
}
