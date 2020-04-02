using System;
using System.Collections.Generic;
using System.Data;

namespace Dahl.Data.Common
{
    public interface IDatabase
    {
        string             ConnectionStringName { get; set; }
        string             ConnectionString { get; set; }
        string             ProviderName { get; set; }
        int                CommandTimeOut { get; set; }

        IDbConnection      Connection { get; set; }
        IDbTransaction     Transaction { get; set; }

        List<DataProvider> ProviderList { get; }
        LastError          LastError { get; set; }

        void Close();

        #region CreateParameters
        IDbDataParameter CreateOutputParameter( string name, Type type );

        IDbDataParameter CreateParameter( string name, object value, Type type, bool isNullable = false );
        IDbDataParameter CreateParameter( string name, object value, Type type, int maxLen, bool isNullable = false );

        IDbDataParameter CreateParameter( string name, string value );
        IDbDataParameter CreateParameter( string name, byte value );
        IDbDataParameter CreateParameter( string name, short value );
        IDbDataParameter CreateParameter( string name, int value );
        IDbDataParameter CreateParameter( string name, long value );
        IDbDataParameter CreateParameter( string name, decimal value );
        IDbDataParameter CreateParameter( string name, double value );
        IDbDataParameter CreateParameter( string name, DateTime value );
        IDbDataParameter CreateParameter( string name, Guid value );

        IDbDataParameter CreateParameter( string name, byte? value );
        IDbDataParameter CreateParameter( string name, short? value );
        IDbDataParameter CreateParameter( string name, int? value );
        IDbDataParameter CreateParameter( string name, long? value );
        IDbDataParameter CreateParameter( string name, decimal? value );
        IDbDataParameter CreateParameter( string name, double? value );
        IDbDataParameter CreateParameter( string name, DateTime? value );
        IDbDataParameter CreateParameter( string name, Guid? value );

        IDbDataParameter CreateParameter( string name, byte[] value );
        IDbDataParameter CreateParameter( string name, int[] value );
        #endregion

        void AddParameters( CommandParameter parameters );
        bool CreateCommand( string sqlCmd, CommandType commandType = CommandType.Text );

        //-----------------------------------------------------------------------------------------
        bool CreateQuery( string sqlCmd, CommandParameter parameters = null );
        bool CreateNamedQuery( string storedProcName, CommandParameter parameters = null );

        //-----------------------------------------------------------------------------------------
        T GetReturnValue<T>( string name );

        //-----------------------------------------------------------------------------------------
        int ExecuteCommand( string sqlCmd, CommandParameter parameters = null );
        int ExecuteNamedCommand( string sqlCmd, CommandParameter parameters = null );

        //-----------------------------------------------------------------------------------------
        IEnumerable<TEntity> ExecuteQuery<TEntity>( string sqlCmd, IMapper<TEntity> mapper = null ) where TEntity : class, new();
        IEnumerable<TEntity> ExecuteQuery<TEntity>( string sqlCmd, CommandParameter parameters, IMapper<TEntity> mapper = null ) where TEntity : class, new();
        IDataReader ExecuteMultipleQueries( string sqlCmd, CommandParameter parameters = null );

        IEnumerable<TEntity> ExecuteNamedQuery<TEntity>( string sqlCmd, IMapper<TEntity> mapper = null ) where TEntity : class, new();
        IEnumerable<TEntity> ExecuteNamedQuery<TEntity>( string sqlCmd, CommandParameter parameters, IMapper<TEntity> mapper = null ) where TEntity : class, new();

        //-----------------------------------------------------------------------------------------
        IEnumerable<TEntity> Read<TEntity>( IMapper<TEntity> mapper = null ) where TEntity : class, new();

        bool BulkInsert<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper ) where TEntity : class, new();
        bool BulkCopy<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper ) where TEntity : class, new();
        bool BulkUpdate<TEntity>( IEnumerable<TEntity> list, IBulkMapper bulkMapper ) where TEntity : class, new();
        void Dispose();
    }
}
