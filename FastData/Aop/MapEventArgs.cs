using System;
using System.Collections.Generic;
using System.Data.Common;

namespace FastData.Aop
{
    public class MapEventArgs : EventArgs
    {
        public string dbType;
        public string sql;
        public string mapName;
        public List<DbParameter> param;

        public MapEventArgs(string dbType, string mapName, string sql, List<DbParameter> param)
        {
            this.dbType = dbType;
            this.mapName = mapName;
            this.sql = sql;
            this.param = param;
        }
    }
}
