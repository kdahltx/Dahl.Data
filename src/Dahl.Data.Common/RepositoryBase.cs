using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    public abstract class RepositoryBase : IRepositoryBase
    {
        private IDatabase _database;
        public IDatabase Database
        {
            get { return _database ?? ( _database = CreateDatabase() ); }
            set { _database = value; }
        }

        protected abstract IDatabase CreateDatabase();

        public void Close()
        {
            _database?.Close();
        }

        private string _connectionString;
        protected virtual string GetConnectionString( string connectionStringName )
        {
            if ( _connectionString.IsNotNullOrEmpty() )
                return _connectionString;

            var css = new Dahl.Data.Common.ConnectionStringSettings( connectionStringName );
            _connectionString = css.ConnectionString;
            return _connectionString;
        }

        #region CreateParameter Methods -----------------------------------------------------------
        public IDbDataParameter CreateOutputParameter( string name, Type type )
        {
            return Database.CreateOutputParameter( name, type );
        }

        /// <summary>
        /// CreateParameter overload
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDbDataParameter CreateParameter( string name, string value )
        {
            return Database.CreateParameter( name, value, typeof( string ), true );
        }

        public IDbDataParameter CreateParameter( string name, string value, int maxLen )
        {
            if ( value.IsNullOrEmpty() )
                return Database.CreateParameter( name, null, typeof( string ), maxLen, true );

            if ( value.Length > maxLen )
                Trace.WriteLine( $"Common.RepositoryBase.CreateParameter name: [{name}], maxLen:[{maxLen}], actual length: [{name.Length}]" );

            return Database.CreateParameter( name, value, typeof( string ), maxLen, true );
        }

        public IDbDataParameter CreateParameter( string name, bool value )
        {
            return Database.CreateParameter( name, value, typeof( bool ) );
        }

        public IDbDataParameter CreateParameter( string name, byte value )
        {
            return Database.CreateParameter( name, value, typeof( byte ) );
        }

        public IDbDataParameter CreateParameter( string name, byte[] value, int maxLen )
        {
            return Database.CreateParameter( name, value, typeof( byte[] ), maxLen );
        }

        public IDbDataParameter CreateParameter( string name, short value )
        {
            return Database.CreateParameter( name, value, typeof( short ) );
        }

        public IDbDataParameter CreateParameter( string name, int value )
        {
            return Database.CreateParameter( name, value, typeof( int ) );
        }

        public IDbDataParameter CreateParameter( string name, int[] value, int maxLen )
        {
            return Database.CreateParameter( name, value, typeof( int[] ), maxLen );
        }

        public IDbDataParameter CreateParameter( string name, long value )
        {
            return Database.CreateParameter( name, value, typeof( long ) );
        }

        public IDbDataParameter CreateParameter( string name, decimal value, byte precision = 0, byte scale = 0 )
        {
            return Database.CreateParameter( name, value, precision, scale );
        }

        public IDbDataParameter CreateParameter( string name, double value )
        {
            return Database.CreateParameter( name, value, typeof( double ) );
        }

        public IDbDataParameter CreateParameter( string name, DateTime value )
        {
            return Database.CreateParameter( name, value, typeof( DateTime ) );
        }

        public IDbDataParameter CreateParameter( string name, Guid value )
        {
            return Database.CreateParameter( name, value, typeof( Guid ) );
        }

        public IDbDataParameter CreateParameter( string name, bool? value )
        {
            return Database.CreateParameter( name, value, typeof( bool ), true );
        }

        public IDbDataParameter CreateParameter( string name, byte? value )
        {
            return Database.CreateParameter( name, value, typeof( byte ), true );
        }

        public IDbDataParameter CreateParameter( string name, short? value )
        {
            return Database.CreateParameter( name, value, typeof( short ), true );
        }

        public IDbDataParameter CreateParameter( string name, int? value )
        {
            return Database.CreateParameter( name, value, typeof( int ), true );
        }

        public IDbDataParameter CreateParameter( string name, long? value )
        {
            return Database.CreateParameter( name, value, typeof( long ), true );
        }

        public IDbDataParameter CreateParameter( string name, decimal? value )
        {
            return Database.CreateParameter( name, value, typeof( decimal ), true );
        }

        public IDbDataParameter CreateParameter( string name, double? value )
        {
            return Database.CreateParameter( name, value, typeof( double ), true );
        }

        public IDbDataParameter CreateParameter( string name, DateTime? value )
        {
            return Database.CreateParameter( name, value, typeof( DateTime ), true );
        }

        public IDbDataParameter CreateParameter( string name, Guid? value )
        {
            return Database.CreateParameter( name, value, typeof( Guid ), true );
        }
        #endregion
    }
}
