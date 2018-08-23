using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Dahl.Extensions;
using Dahl.Data.Common;
using System.Data.Common;

namespace Dahl.Data.SqlServer
{
    public class Database : Dahl.Data.Common.Database
    {
        public Database()
        {
            ProviderName = "System.Data.SqlClient";
        }

        protected SqlCommand Cmd
        {
            get { return _cmd as SqlCommand; }
            set { _cmd = value; }
        }

        protected SqlDataReader Reader
        {
            get { return _reader as SqlDataReader; }
            set { _reader = value; }
        }

        protected SqlDataAdapter DataAdapter
        {
            get { return _da as SqlDataAdapter; }
            set { _da = value; }
        }

        // https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
        protected override DbType GetDbType( Type dataType )
        {
            switch ( dataType.ToString() )
            {
                case "System.Int64": return (DbType)SqlDbType.BigInt;
                case "System.Byte[]": return (DbType)SqlDbType.Binary;
                case "System.Boolean": return (DbType)SqlDbType.Bit;
                case "System.Char": return (DbType)SqlDbType.Char;
                case "System.DateTime": return (DbType)SqlDbType.DateTime;
                case "System.Decimal": return (DbType)SqlDbType.Decimal;
                case "System.Double": return (DbType)SqlDbType.Decimal;
                case "System.Int16": return (DbType)SqlDbType.SmallInt;
                case "System.Int32": return (DbType)SqlDbType.Int;
                case "System.String": return (DbType)SqlDbType.VarChar;
                case "System.Single": return (DbType)SqlDbType.Float;
                case "System.double": return (DbType)SqlDbType.Decimal;
                case "System.Guid": return (DbType)SqlDbType.UniqueIdentifier;
                default: return DbType.String;
            }
        }

        private SqlDbType GetSqlDbType( Type dataType )
        {
            return (SqlDbType)GetDbType( dataType );
        }

        public override IDbDataParameter CreateParameter( string name, object value, Type type, bool isNullable = false )
        {
            IDbDataParameter parameter = new SqlParameter( name, value )
            {
                SqlDbType = GetSqlDbType( type ),
                IsNullable = isNullable,
                Size = GetSize( value )
            };
            return parameter;
        }

        #region BulkCopy Methods ------------------------------------------------------------------
        public override bool BulkInsert<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper )
        {
            try
            {
                CreateConnection();
                SqlConnection connection = Connection as SqlConnection;
                if ( connection == null )
                    throw new NullReferenceException( nameof( Connection ) );

                SqlBulkCopy bulkCopy = new SqlBulkCopy( connection );
                bulkCopy.DestinationTableName = bulkMapper.DstTableName[0];

                foreach ( string map in bulkMapper.MapList )
                {
                    string[] cols = map.Split( ',' );
                    bulkCopy.ColumnMappings.Add( cols[0], cols[1] );
                }

                using ( EntityReader<TEntity> er = new EntityReader<TEntity>( list ) )
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    bulkCopy.WriteToServer( er );
                    sw.Stop();
                    bulkCopy.Close();

                    Trace.WriteLine( $"bulkCopy took {sw.ElapsedMilliseconds} ms to execute" );
                }
            }
            catch ( Exception e )
            {
                LastError.Code = e.HResult;
                LastError.Message = e.Message;
                return false;
            }

            return true;
        }

        /* 
        Example 1
            Merges the DailySales table into GlobalSales:

            merge into GlobalSales
                (Item_number, Description, Quantity)as G
            using DailySales as D
            ON D.Item_number = G.Item_number
            when not matched then
                insert (Item_number, Description, Quantity )
                    values (D.Item_number, D.Description, D.Quantity)
            when matched then 
                update set G.Quantity = G.Quantity + D.Quantity

        Example 2
        Uses a derived table as the source table with dynamic parameter markers:

        merge into GlobalSales (Item_number, Description, Quantity)as G
        using select (?, ?, ?) as D (Item_number, Description, Quantity)
        ON D.Item_number = G.Item_number
        when not matched then
            insert (Item_number, Description, Quantity )
                  values (D.Item_number, D.Description, D.Quantity)
        when matched then 
            update set G.Quantity = G.Quantity + D.Quantity
        */
        //-----------------------------------------------------------------------------------------
        public override bool BulkUpdate<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper )
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                if ( bulkMapper.SqlCreateTmpTable.IsNotNullOrEmpty() )
                {
                    CreateCommand( bulkMapper.SqlCreateTmpTable );
                    Cmd.ExecuteNonQuery();
                }

                // this is the connection for the bulkupdate
                SqlConnection connection = Connection as SqlConnection;
                if ( connection == null )
                    throw new NullReferenceException( nameof( connection ) );

                SqlBulkCopy bulkCopy = new SqlBulkCopy( connection );
                bulkCopy.DestinationTableName = $"#tmp_{bulkMapper.TmpTableName}";
                foreach ( string colPair in bulkMapper.MapList )
                {
                    string[] fields = colPair.Split( ',' );
                    if ( fields.Length == 2 )
                        bulkCopy.ColumnMappings.Add( fields[0], fields[0] );
                }
#if false
                using ( var reader = ObjectReader.Create( list ) )
                {
                    // write records to temp table.
                    sw.Restart();
                    bulkCopy.WriteToServer( reader );
                    sw.Stop();
                    bulkCopy.Close();
                    Trace.WriteLine( $"BulkUpdate bulkCopy.WriteToServer took {sw.ElapsedMilliseconds} ms to execute" );

                    // merge temp table into specified tables, one at a time.
                    foreach ( var sqlMerge in bulkMapper.SqlMerge )
                    {
                        var resultList = ExecuteQuery<TEntity>( $"select * from #tmp_{bulkMapper.TmpTableName}" );
                        if ( resultList != null && resultList.Any() )
                        {
                            if ( sqlMerge.IsNotNullOrEmpty() )
                            {
                                sw.Restart();
                                CreateCommand( sqlMerge );
                                Cmd.ExecuteNonQuery();
                                sw.Stop();
                                Trace.WriteLine( $"BulkUpdate Merge statement took {sw.ElapsedMilliseconds} ms to execute" );
                            }
                        }
                    }
                }
#else
                using ( EntityReader<TEntity> reader = new EntityReader<TEntity>( list ) )
                {
                    // write records to temp table.
                    sw.Restart();
                    bulkCopy.WriteToServer( reader );
                    sw.Stop();
                    bulkCopy.Close();
                    Trace.WriteLine( $"BulkUpdate bulkCopy.WriteToServer took {sw.ElapsedMilliseconds} ms to execute" );

                    // merge temp table into specified tables, one at a time.
                    foreach ( var sqlMerge in bulkMapper.SqlMerge )
                    {
                        var resultList = ExecuteQuery<TEntity>( $"select top 1 * from #tmp_{bulkMapper.TmpTableName}" );
                        if ( resultList != null && resultList.Any() )
                        {
                            if ( sqlMerge.IsNotNullOrEmpty() )
                            {
                                CreateCommand( sqlMerge );
                                sw.Restart();
                                Cmd.ExecuteNonQuery();
                                sw.Stop();
                                Trace.WriteLine( $"BulkUpdate Merge statement took {sw.ElapsedMilliseconds} ms to execute" );
                            }
                        }
                    }
                }
#endif
            }
            catch ( Exception e )
            {
                LastError.Code = e.HResult;
                LastError.Message = e.Message;
                Trace.WriteLine( $"Error Message: {e.Message}" );
                return false;
            }

            return true;
        }

        //-----------------------------------------------------------------------------------------
        // get schema information to get property types so we can build insert 
        // and update statements
        public string CreateMergeSqlStatement( PropertyInfo[] properties, string tableName )
        {
            StringBuilder sbCreateTable = new StringBuilder();
            StringBuilder sbColNames = new StringBuilder();
            StringBuilder sbColValues = new StringBuilder();

            sbCreateTable.Append( $"create table #tmp_{tableName} (" );
            foreach ( var prop in properties )
            {
                sbCreateTable.Append( $"{prop.Name}  {GetSqlDbType( prop.PropertyType )}," );
                sbColNames.Append( $"{prop.Name}," );
                sbColValues.Append( $"src.{prop.Name}," );
            }

            return string.Empty;
        }

        //-----------------------------------------------------------------------------------------
        private string[] GetColumnNames( PropertyInfo[] properties )
        {
            string[] objectCols = new string[properties.Length];
            int i = 0;

            foreach ( PropertyInfo prop in properties )
            {
                if ( prop.PropertyType.FullName.StartsWith( "System." ) )
                    objectCols[i++] = prop.Name;
            }

            return objectCols;
        }

        //-----------------------------------------------------------------------------------------
        public bool BulkUpsert<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper )
        {
            try
            {
                CreateConnection();
                SqlConnection connection = Connection as SqlConnection;
                if ( connection == null )
                    throw new NullReferenceException( nameof( Connection ) );

                SqlBulkCopy bulkCopy = new SqlBulkCopy( connection );
                bulkCopy.DestinationTableName = bulkMapper.DstTableName[0];

                foreach ( string map in bulkMapper.MapList )
                {
                    string[] cols = map.Split( ',' );
                    bulkCopy.ColumnMappings.Add( cols[0], cols[1] );
                }

                using ( EntityReader<TEntity> er = new EntityReader<TEntity>( list ) )
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    bulkCopy.WriteToServer( er );
                    sw.Stop();
                    bulkCopy.Close();

                    Trace.WriteLine( $"bulkCopy took {sw.ElapsedMilliseconds} ms to execute" );
                }
            }
            catch ( Exception e )
            {
                LastError.Code = e.HResult;
                LastError.Message = e.Message;
                return false;
            }

            return true;
        }

        public override bool BulkCopy<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper )
        {
            try
            {
                CreateConnection();
                SqlConnection connection = Connection as SqlConnection;
                if ( connection == null )
                    throw new NullReferenceException( nameof( Connection ) );

                SqlBulkCopy bulkCopy = new SqlBulkCopy( connection );
                bulkCopy.DestinationTableName = bulkMapper.DstTableName[0];

                foreach ( string map in bulkMapper.MapList )
                {
                    string[] cols = map.Split( ',' );
                    bulkCopy.ColumnMappings.Add( cols[0], cols[1] );
                }

                using ( EntityReader<TEntity> er = new EntityReader<TEntity>( list ) )
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    bulkCopy.WriteToServer( er );
                    sw.Stop();
                    bulkCopy.Close();

                    Trace.WriteLine( $"bulkCopy took {sw.ElapsedMilliseconds} ms to execute" );
                }
            }
            catch ( Exception e )
            {
                LastError.Code = e.HResult;
                LastError.Message = e.Message;
                return false;
            }

            return true;
        }
        #endregion

#if NETCOREAPP2_0
        protected override DbProviderFactory CreateProviderFactory()
        {
            return SqlClientFactory.Instance;
        }
#endif
    }
}

