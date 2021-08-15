using System;
using System.Collections.Generic;
using System.Data.Common;

namespace FastData.Aop
{
    public class BeforeEventArgs : EventArgs
    {
        public string dbType;
        public List<string> tableName;
        public string sql;
        public List<DbParameter> param;
        public bool isRead = false;
        public bool isWrite = true;

        public BeforeEventArgs(string dbType, List<string> tableName, string sql, List<DbParameter> param, bool isRead = true)
        {
            this.dbType = dbType;
            this.tableName = tableName ?? new List<string>();
            this.sql = sql;
            this.param = param ?? new List<DbParameter>();
            this.isRead = isRead;
            this.isWrite = !isRead;
        }
    }
}
