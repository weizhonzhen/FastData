using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Property
{
    /// <summary>
    /// 字段类型
    /// </summary>
    public class DbType
    {
        public enum Oracle
        {
            Char = 1,
            Varchar2 = 2,
            Nchar = 3,
            Nvarchar2 = 4,
            Date = 5,
            Long = 6,
            Raw = 7,
            Blob = 8,
            Clob = 9,
            Nclob = 10,
            Bfile = 11,
            Rowid = 12,
            Nrowid = 13,
            Integer = 14,
            Float = 15,
            Decimal = 16,
            Real = 17,
            Number = 18
        }

        public enum SqlServer
        {
            Bit = 0,
            Tinyint = 1,
            Smallint = 2,
            Int = 3,
            Bigint = 4,
            Real = 5,
            Float = 6,
            Money = 7,
            DateTime = 8,
            Char = 9,
            Varchar = 10,
            Nchar = 11,
            Nvarchar = 12,
            Text = 13,
            Ntext = 14,
            Image = 15,
            Binary = 16,
            Uniqueidentifier = 17
        }

        public enum MySql
        {
            Char = 0,
            Varchar = 1,
            Tinyblob = 2,
            Tinytext = 3,
            Blog = 4,
            Text = 5,
            Mediumblog = 6,
            Mediumtext = 7,
            Longblob = 8,
            Longtext = 9,
            Varbinary = 10,
            Binary = 11
        }
    }
}
