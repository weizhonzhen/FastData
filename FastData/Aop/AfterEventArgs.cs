using System;
using System.Collections.Generic;
using System.Data.Common;

namespace FastData.Aop
{
    public class AfterEventArgs : EventArgs
    {
        public string dbType ;
        public List<string> tableName;
        public string sql;
        public List<DbParameter> param;
        public object result;
        public bool isRead = false;
        public bool isWrite = true;

        public AfterEventArgs(string dbType, List<string> tableName, string sql, List<DbParameter> param, object result, bool isRead=true)
        {
            this.dbType = dbType;
            this.tableName = tableName ?? new List<string>();
            this.sql = sql;
            this.param = param;
            this.result = result;
            this.isRead = isRead;
            this.isWrite = !isRead;
        }
    }
}
