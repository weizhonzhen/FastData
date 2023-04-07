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
        public readonly static string Oracle = "Oracle.ManagedDataAccess";
        public readonly static string OracleFactory = "Oracle.ManagedDataAccess.Client.OracleClientFactory";

        /// <summary>
        /// mysql
        /// </summary>
        public readonly static string SqlServer = "System.Data";
        public readonly static string SqlServerFactory = "System.Data.SqlClient.SqlClientFactory";

        /// <summary>
        /// sql server
        /// </summary>
        public readonly static string MySql = "MySql.Data";
        public readonly static string MySqlFactory = "MySql.Data.MySqlClient.MySqlClientFactory";

        /// <summary>
        /// SQLite
        /// </summary> 
        public readonly static string SQLite = "Microsoft.Data.Sqlite";
        public readonly static string SQLiteFactory = "Microsoft.Data.Sqlite.SqliteFactory";
       
        /// <summary>
        /// DB2
        /// </summary>
        public readonly static string DB2 = "IBM.FastData.DB2.iSeries";
        public readonly static string DB2Factory = "MySql.Data.MySqlClient.MySqlClientFactory";

        /// <summary>
        /// PostgreSql test
        /// </summary>
        public readonly static string PostgreSql = "PostgreSql";
        public readonly static string PostgreSqlFactory = "Npgsql.NpgsqlFactory";
    }
}
