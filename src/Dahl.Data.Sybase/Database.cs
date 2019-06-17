using System;
using System.Data;
#if !NETCOREAPP2_0 && !NETCOREAPP2_1 && !NETCOREAPP2_2
using Sybase.Data.AseClient;
#else
using AdoNetCore.AseClient;
#endif

namespace Dahl.Data.Sybase
{
    public class Database : Data.Common.Database
    {
        public Database()
        {
            ProviderName = "Sybase.Data.AseClient";
        }

        protected AseCommand Cmd
        {
            get { return _Cmd as AseCommand; }
            set { _Cmd = value; }
        }

        protected AseDataReader Reader
        {
            get { return _Reader as AseDataReader; }
            set { _Reader = value; }
        }

        protected AseDataAdapter DataAdapter
        {
            get { return _Da as AseDataAdapter; }
            set { _Da = value; }
        }

        // https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
        protected override DbType GetDbType(Type dataType)
        {
            switch (dataType.ToString())
            {
                case "System.Int64": return (DbType)AseDbType.BigInt;
                case "System.Byte[]": return (DbType)AseDbType.Binary;
                case "System.Boolean": return (DbType)AseDbType.Bit;
                case "System.Char": return (DbType)AseDbType.Char;
                case "System.DateTime": return (DbType)AseDbType.DateTime;
                case "System.Decimal": return (DbType)AseDbType.Decimal;
                case "System.Double": return (DbType)AseDbType.Decimal;
                case "System.Int16": return (DbType)AseDbType.SmallInt;
                case "System.Int32": return (DbType)AseDbType.Integer;
                case "System.String": return (DbType)AseDbType.VarChar;
                case "System.Single": return (DbType)AseDbType.Real;
                case "System.double": return (DbType)AseDbType.Decimal;
                case "System.Guid": return (DbType)AseDbType.VarChar;
                default: return DbType.String;
            }
        }

        private AseDbType GetAselDbType(Type dataType)
        {
            return (AseDbType)GetDbType(dataType);
        }

        public override IDbDataParameter CreateParameter(string name, object value, Type type, bool isNullable = false)
        {
            IDbDataParameter parameter = new AseParameter(name, value)
            {
                AseDbType = GetAselDbType(type),
                IsNullable = isNullable,
                Size = GetSize(value)
            };
            return parameter;
        }

    }
}
