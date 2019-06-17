using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    public abstract class Database : IDisposable, IDatabase
    {
        protected const string _CreateConnectionError = "Unable to create IConnection object";
        protected const string _UnableToOpenDbError   = "Unable to open database connection";

        public IDbConnection  Connection     { get; set; }
        public int            CommandTimeOut { get; set; } = 300;
        public IDbTransaction Transaction    { get; set; }
        public LastError      LastError      { get; set; } = new LastError();

        public string ConnectionStringName { get; set; }
        public string ConnectionString     { get; set; }
        public string ProviderName         { get; set; }

        #region Database Providers ----------------------------------------------------------------
#if !NETCOREAPP2_0 && !NETCOREAPP2_1 && !NETCOREAPP2_2
        private List<DataProvider> _providerList;
        public  List<DataProvider> ProviderList { get { return _providerList ?? ( _providerList = GetProviderFactoryClasses() ); } }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<DataProvider> GetProviderFactoryClasses()
        {
            List<DataProvider> list = new List<DataProvider>();
            DataTable          dt   = DbProviderFactories.GetFactoryClasses();

            foreach ( DataRow dr in dt.Rows )
            {
                Trace.WriteLine( dr.ItemArray[0].AsString() );
                DataProvider dp = new DataProvider()
                {
                    Name                  = dr.ItemArray[0].As<string>(),
                    Description           = dr.ItemArray[1].As<string>(),
                    InvariantName         = dr.ItemArray[2].As<string>(),
                    AssemblyQualifiedName = dr.ItemArray[3].As<string>()
                };
                list.Add( dp );
            }

            return list;
        }
#else
        private List<DataProvider> _providerList;
        public List<DataProvider> ProviderList
        {
            get { return _providerList ?? ( _providerList = GetProviderFactoryClasses() ); }
        }
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<DataProvider> GetProviderFactoryClasses()
        {
            List<DataProvider> list = new List<DataProvider>();
            //DataTable dt = DbProviderFactories.GetFactoryClasses();

            //foreach ( DataRow dr in dt.Rows )
            //{
            //    Trace.WriteLine( dr.ItemArray[0].AsString() );
            //    DataProvider dp = new DataProvider()
            //    {
            //        Name = dr.ItemArray[0].As<string>(),
            //        Description = dr.ItemArray[1].As<string>(),
            //        InvariantName = dr.ItemArray[2].As<string>(),
            //        AssemblyQualifiedName = dr.ItemArray[3].As<string>()
            //    };
            //    list.Add( dp );
            //}

            return list;
        }
#endif
        #endregion

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetConnectionStringName()
        {
            if ( ConnectionStringName.IsNullOrEmpty() )
                throw new NullReferenceException( "ConnectionStringName is undefined. Either set ConnectionStringName " +
                                                  "or override medthod GetConnectionStringName()" );

            return ConnectionStringName;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetConnectionString()
        {
            throw new NotImplementedException( "GetConnectionString()" );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations" )]
        protected DbProviderFactory ProviderFactory
        {
            get { return _providerFactory ?? ( _providerFactory = CreateProviderFactory() ); }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual DbProviderFactory CreateProviderFactory()
        {
            try
            {
#if !NETCOREAPP2_0 && !NETCOREAPP2_1 && !NETCOREAPP2_2
                _providerFactory = DbProviderFactories.GetFactory( ProviderName );
#else
                throw new NotImplementedException("This method must be overridden in a .NET Core Applications");
#endif
            }
            catch ( Exception e )
            {
                LastError.Code    = e.HResult;
                LastError.Message = e.Message;
            }

            return _providerFactory;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        protected bool CreateConnection()
        {
            ClearLastError();
            string fnName = GetMethodName();
            try
            {
                if ( ProviderName.IsNullOrEmpty() )
                    throw new NullReferenceException( nameof( ProviderName ) );

                if ( Connection == null && ProviderFactory != null )
                    Connection = ProviderFactory.CreateConnection();

                if ( Connection != null )
                {
                    if ( Connection.State == ConnectionState.Open )
                        return true;

                    Connection.ConnectionString = ConnectionString;
                    Connection.Open();

                    if ( Connection.State == ConnectionState.Open )
                        return true;

                    SetLastError( 1001, FormatExceptionMessage( GetMethodName(), _CreateConnectionError, null ) );
                }
            }
            catch ( DataException ex )
            {
                SetLastError( 1008, FormatExceptionMessage( fnName, _UnableToOpenDbError, ex ) );
            }
            catch ( Exception ex )
            {
                SetLastError( 1009, FormatExceptionMessage( fnName, _CreateConnectionError, ex ) );
            }

            return false;
        }

        ~Database()
        {
            Trace.WriteLine( $"0:Start {GetType().FullName}.~Database()" );
            try
            {
                Dispose();
            }
            catch ( Exception e )
            {
                LastError.Message = GetExceptionAsString( e );
                Trace.WriteLine( $"Logging Exception: {LastError.Message}" );
            }
            finally
            {
                GC.SuppressFinalize( this );
                Trace.WriteLine( $"0:End   {GetType().FullName}.~Database()" );
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            Trace.WriteLine( $"0:Start {GetType().FullName}.Dispose()" );
            Dispose( true );
            Trace.WriteLine( $"0:End   {GetType().FullName}.Dispose()" );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose( bool disposing )
        {
            Trace.WriteLine( $"0:Start {GetType().FullName}.Dispose({disposing})" );
            try
            {
                if ( _disposed )
                    return;

                _disposed = true;
                if ( disposing )
                    Close();
            }
            catch ( Exception e )
            {
                LastError.Message = GetExceptionAsString( e );
                Trace.WriteLine( $"Logging Exception: {LastError.Message}" );
            }

            Trace.WriteLine( $"0:End   {GetType().FullName}.Dispose({disposing})" );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public virtual void Close()
        {
            Trace.WriteLine( $"0: Start {GetType().FullName}.Close()" );
            try
            {
                if ( Connection != null )
                {
                    Trace.WriteLine( $"0: {GetType().FullName}.Close() - Connection.State = {Connection.State}." );
                    if ( Connection.State != ConnectionState.Closed )
                    {
                        Connection.Close(); // TODO: looks like this statement is thowing an exception for 4.6x,4.7x
                        Trace.WriteLine( $"0: {GetType().FullName}.Close() - Post Connection.Close() call" );
                        Connection = null;
                    }
                }
            }
            catch ( Exception e )
            {
                LastError.Code    = 1000;
                LastError.Message = GetExceptionAsString( e );
                Trace.WriteLine( $"Logging Exception: {LastError.Message} " );
            }

            Trace.WriteLine( $"0: End {GetType().FullName}.Close()" );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected string GetExceptionAsString( Exception e )
        {
            StringBuilder sb = new StringBuilder();
            Exception     ex = e;
            while ( ex != null )
            {
                sb.AppendLine( e.Message );
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        #region Parameters ------------------------------------------------------------------------
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        protected virtual DbType GetDbType( Type dataType )
        {
            switch ( dataType.ToString() )
            {
                case "System.Char" :     return DbType.AnsiStringFixedLength;
                case "System.Boolean" :  return DbType.Boolean;
                case "System.Int16" :    return DbType.Int16;
                case "System.Int32" :    return DbType.Int32;
                case "System.String" :   return DbType.String;
                case "System.Single" :   return DbType.Single;
                case "System.double" :   return DbType.Double;
                case "System.Decimal" :  return DbType.Decimal;
                case "System.Int64" :    return DbType.Decimal;
                case "System.DateTime" : return DbType.DateTime;
                case "System.Guid" :     return DbType.Guid;
                case "System.Byte[]" :   return DbType.Binary;
                default :                return DbType.String;
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected virtual int GetSize( object val )
        {
            if ( val == null )
                return 0;

            switch ( val.GetType().ToString() )
            {
                case "System.String" :  return ( (string)val ).Length;
                case "System.Char" :    return sizeof( char );
                case "System.Boolean" : return sizeof( bool );
                case "System.Int16" :   return sizeof( short );
                case "System.Int32" :   return sizeof( int );
                case "System.Single" :  return sizeof( float );
                case "System.double" :  return sizeof( double );
                case "System.Decimal" : return sizeof( decimal );
                case "System.Int64" :   return sizeof( long );

                case "System.Guid" :
                case "System.DateTime" :
                case "System.Byte[]" : return 0;
            }

            return 0;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="isNullable"></param>
        /// <returns></returns>
        public virtual IDbDataParameter CreateParameter( string name, object value, Type type, bool isNullable = false )
        {
            throw new NotImplementedException( "CreateParameter" );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IDbDataParameter CreateParameter( string name, string value )
        {
            return CreateParameter( name, value, typeof( string ), true );
        }

        #region CreateParameter short -------------------------------------------------------------
        public IDbDataParameter CreateParameter( string name, short value )
        {
            return CreateParameter( name, value, typeof( short ), true );
        }

        public IDbDataParameter CreateParameter( string name, short? value )
        {
            return CreateParameter( name, value, typeof( short? ), true );
        }
        #endregion

        #region CreateParameter int ---------------------------------------------------------------
        public virtual IDbDataParameter CreateParameter( string name, int value )
        {
            return CreateParameter( name, value, typeof( int ) );
        }

        public virtual IDbDataParameter CreateParameter( string name, int? value )
        {
            return CreateParameter( name, value, typeof( int? ), true );
        }
        #endregion

        #region CreateParameter long --------------------------------------------------------------
        public IDbDataParameter CreateParameter( string name, long value )
        {
            return CreateParameter( name, value, typeof( long ) );
        }

        public IDbDataParameter CreateParameter( string name, long? value )
        {
            return CreateParameter( name, value, typeof( long? ), true );
        }
        #endregion

        #region CreateParameter decimal -----------------------------------------------------------
        public virtual IDbDataParameter CreateParameter( string name, decimal value )
        {
            return CreateParameter( name, value, typeof( decimal ) );
        }

        public virtual IDbDataParameter CreateParameter( string name, decimal? value )
        {
            return CreateParameter( name, value, typeof( decimal ), true );
        }
        #endregion

        #region CreateParameter double ------------------------------------------------------------
        public virtual IDbDataParameter CreateParameter( string name, double value )
        {
            return CreateParameter( name, value, typeof( double ) );
        }

        public virtual IDbDataParameter CreateParameter( string name, double? value )
        {
            return CreateParameter( name, value, typeof( double? ), true );
        }
        #endregion

        #region CreateParameter byte --------------------------------------------------------------
        public virtual IDbDataParameter CreateParameter( string name, byte value )
        {
            return CreateParameter( name, value, typeof( byte ) );
        }

        public virtual IDbDataParameter CreateParameter( string name, byte? value )
        {
            return CreateParameter( name, value, typeof( byte? ), true );
        }
        #endregion

        #region CreateParameter DateTime ----------------------------------------------------------
        public virtual IDbDataParameter CreateParameter( string name, DateTime value )
        {
            return CreateParameter( name, value, typeof( DateTime ) );
        }

        public virtual IDbDataParameter CreateParameter( string name, DateTime? value )
        {
            return CreateParameter( name, value, typeof( DateTime? ), true );
        }
        #endregion

        #region CreateParameter Guid --------------------------------------------------------------
        public virtual IDbDataParameter CreateParameter( string name, Guid value )
        {
            return CreateParameter( name, value, typeof( Guid ) );
        }

        public virtual IDbDataParameter CreateParameter( string name, Guid? value )
        {
            return CreateParameter( name, value, typeof( Guid? ), true );
        }
        #endregion
        #endregion

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Adds parameters to the IDbCommand Parameters property.
        /// </summary>
        /// <param name="parameters"></param>
        public void AddParameters( CommandParameter parameters )
        {
            if ( _Cmd == null || parameters == null )
                return;

            _Cmd.Parameters.Clear();
            parameters.AddParameters( _Cmd );
            _Cmd.Prepare();
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public bool CreateCommand( string sqlCmd, CommandType commandType = CommandType.Text )
        {
            try
            {
                if ( CreateConnection() )
                {
                    _Cmd                = Connection.CreateCommand();
                    _Cmd.CommandTimeout = CommandTimeOut;
                    _Cmd.CommandText    = sqlCmd;
                    _Cmd.CommandType    = commandType;
                    _Cmd.Parameters.Clear();

                    return true;
                }
            }
            catch ( Exception ex )
            {
                SetLastError( 1000, ex.Message );
            }

            return false;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual bool CreateQuery( string sqlCmd, CommandParameter parameters = null )
        {
            bool result = CreateCommand( sqlCmd );
            if ( result )
                AddParameters( parameters );

            return result;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual bool CreateNamedQuery( string storedProcName, CommandParameter parameters = null )
        {
            bool result = CreateCommand( storedProcName, CommandType.StoredProcedure );
            if ( result )
                AddParameters( parameters );

            return result;
        }

        #region ExecuteCommand --------------------------------------------------------------------
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteCommand( string sqlCmd, CommandParameter parameters = null )
        {
            int numRows = 0;
            try
            {
                if ( CreateQuery( sqlCmd, parameters ) )
                    numRows = _Cmd.ExecuteNonQuery();
            }
            catch ( DataException e )
            {
                SetLastError( 1002, e.Message );
            }
            catch ( Exception e )
            {
                SetLastError( 1003, e.Message );
            }

            return numRows;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteNamedCommand( string sqlCmd, CommandParameter parameters = null )
        {
            int numRows = 0;
            try
            {
                if ( CreateNamedQuery( sqlCmd, parameters ) )
                    numRows = _Cmd.ExecuteNonQuery();
            }
            catch ( DataException ex )
            {
                SetLastError( 1002, ex.Message );
            }

            return numRows;
        }
        #endregion

        #region ExecuteQuery ----------------------------------------------------------------------
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlCmd"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> ExecuteQuery<TEntity>( string sqlCmd, IMapper<TEntity> mapper = null )
            where TEntity : class, new()
        {
            try
            {
                if ( CreateQuery( sqlCmd ) )
                {
                    _Reader = _Cmd.ExecuteReader();

                    if ( _Reader != null )
                        return Read( mapper );
                }
            }
            catch ( Exception ex )
            {
                SetLastError( 1003, ex.Message );
            }

            return null;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlCmd"></param>
        /// <param name="parameters"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> ExecuteQuery<TEntity>( string           sqlCmd, CommandParameter parameters,
                                                                   IMapper<TEntity> mapper = null )
            where TEntity : class, new()
        {
            try
            {
                if ( CreateQuery( sqlCmd, parameters ) )
                {
                    _Reader = _Cmd.ExecuteReader();

                    if ( _Reader != null )
                        return Read( mapper );
                }
            }
            catch ( Exception ex )
            {
                SetLastError( 1003, ex.Message );
            }

            return null;
        }
        #endregion

        #region ExecuteNamedQuery -----------------------------------------------------------------
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlCmd"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> ExecuteNamedQuery<TEntity>( string sqlCmd, IMapper<TEntity> mapper = null )
            where TEntity : class, new()
        {
            try
            {
                if ( CreateNamedQuery( sqlCmd ) )
                {
                    _Reader = _Cmd.ExecuteReader();
                    if ( _Reader != null )
                    {
                        var result = Read( mapper );
                        _Reader.NextResult();

                        return result;
                    }
                }
            }
            catch ( Exception ex )
            {
                SetLastError( 1003, ex.Message );
            }

            return null;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlCmd"></param>
        /// <param name="parameters"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> ExecuteNamedQuery<TEntity>( string           sqlCmd, CommandParameter parameters,
                                                                        IMapper<TEntity> mapper = null )
            where TEntity : class, new()
        {
            try
            {
                if ( CreateNamedQuery( sqlCmd, parameters ) )
                {
                    _Reader = _Cmd.ExecuteReader();
                    if ( _Reader != null )
                    {
                        var result = Read( mapper );
                        _Reader.NextResult();

                        return result;
                    }
                }
            }
            catch ( Exception ex )
            {
                SetLastError( 1003, ex.Message );
            }

            return null;
        }
        #endregion

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Read<TEntity>( IMapper<TEntity> mapper = null )
            where TEntity : class, new()
        {
            if ( mapper == null )
                mapper = new Mapper<TEntity>();

            List<TEntity> list = new List<TEntity>();
            while ( _Reader.Read() )
                list.Add( mapper.Map( _Reader ) );

            _Reader.NextResult();

            return list;
        }

        #region BulkCopy Methods ------------------------------------------------------------------
        public virtual bool BulkInsert<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper ) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public virtual bool BulkCopy<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper ) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public virtual bool BulkUpdate<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper ) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Misc Methods ----------------------------------------------------------------------
        protected string FormatExceptionMessage( string functionName, string msg, Exception ex )
        {
            StringBuilder sb = new StringBuilder( 256 );
            sb.Append( $"Error in Database Server.{functionName}()\n" );
            sb.Append( $"Timestamp: {DateTime.Now.ToShortTimeString()} {DateTime.Now.ToShortDateString()}\n" );

            if ( ex != null )
                sb.Append( $"Exception Type: {ex.GetType()}\nException Message: {ex.Message}\n" );

            sb.Append( $"Error Message: {msg}\n" );

            return sb.ToString();
        }

        protected static string GetMethodName()
        {
            StackTrace                   stackTrace = new StackTrace();
            System.Reflection.MethodBase method     = stackTrace.GetFrame( 1 ).GetMethod();

            if ( method == null || method.DeclaringType == null )
                return null;

            return method.DeclaringType.FullName;
        }

        protected void SetLastError( int code, string message )
        {
            LastError.Code    = code;
            LastError.Message = message;
        }

        protected void ClearLastError()
        {
            LastError.Code    = 0;
            LastError.Message = string.Empty;
        }
        #endregion

        #region FIELDS ----------------------------------------------------------------------------
        private bool              _disposed;
        private DbProviderFactory _providerFactory;
        #endregion

        #region FIELDS ----------------------------------------------------------------------------
        protected IDbCommand     _Cmd;
        protected IDataReader    _Reader;
        protected IDbDataAdapter _Da;
        protected DataSet        _Ds;
        #endregion
    }
}
