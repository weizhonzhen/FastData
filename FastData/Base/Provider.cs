using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastData.Base
{
    /// <summary>
    /// 驱动名
    /// </summary>
    public static class Provider
    {
        /// <summary>
        /// oracle
        /// </summary>
        public readonly static string Oracle = "Oracle.ManagedDataAccess.Client";

        /// <summary>
        /// mysql
        /// </summary>
        public readonly static string MySql = "System.Data.SqlClient";

        /// <summary>
        /// sql server
        /// </summary>
        public readonly static string SqlServer = "System.Data.SqlClient";

        /// <summary>
        /// SQLite
        /// </summary>
        public readonly static string SQLite = "System.Data.SQLite";

        /// <summary>
        /// DB2
        /// </summary>
        public readonly static string DB2 = "IBM.FastData.DB2.iSeries";

        /// <summary>
        /// PostgreSql test
        /// </summary>
        public readonly static string PostgreSql = "PostgreSql.Client";
    }
}
