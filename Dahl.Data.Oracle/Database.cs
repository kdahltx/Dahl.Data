using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Dahl.Data.Oracle
{
    public class Database : Dahl.Data.Common.Database
    {
        public Database()
        {
            ProviderName = "Oracle.ManagedDataAccess.Client";
        }

        protected OracleCommand Cmd
        {
            get { return _Cmd as OracleCommand; }
            set { _Cmd = value; }
        }

        protected OracleDataReader Reader
        {
            get { return _Reader as OracleDataReader; }
            set { _Reader = value; }
        }

        protected OracleDataAdapter DataAdapter
        {
            get { return _Da as OracleDataAdapter; }
            set { _Da = value; }
        }

        // https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
        protected override DbType GetDbType(Type dataType)
        {
            switch (dataType.ToString())
            {
                case "System.Int64"   : return (DbType)OracleDbType.Int64;
                case "System.Byte[]"  : return (DbType)OracleDbType.Byte;
                case "System.Boolean" : return (DbType)OracleDbType.Boolean;
                case "System.Char"    : return (DbType)OracleDbType.Char;
                case "System.DateTime": return (DbType)OracleDbType.Date;
                case "System.Decimal" : return (DbType)OracleDbType.Decimal;
                case "System.Double"  : return (DbType)OracleDbType.Decimal;
                case "System.Int16"   : return (DbType)OracleDbType.Int16;
                case "System.Int32"   : return (DbType)OracleDbType.Int32;
                case "System.String"  : return (DbType)OracleDbType.Varchar2;
                case "System.Single"  : return (DbType)OracleDbType.Single;
                case "System.double"  : return (DbType)OracleDbType.Decimal;
                case "System.Guid"    : return (DbType)OracleDbType.Varchar2;
                default               : return DbType.String;
            }
        }

        private OracleDbType GetOracleDbType(Type dataType)
        {
            return (OracleDbType)GetDbType(dataType);
        }

        public override IDbDataParameter CreateParameter(string name, object value, Type type, bool isNullable = false)
        {
            IDbDataParameter parameter = new OracleParameter(name, value)
            {
                OracleDbType = GetOracleDbType(type),
                IsNullable = isNullable,
                Size = GetSize(value)
            };
            return parameter;
        }
    }
}
