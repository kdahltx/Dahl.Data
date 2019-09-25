using System;
using System.Configuration;
using System.Data;
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

        private string _connectionString;
        protected virtual string GetConnectionString( string connectionStringName )
        {
            if ( _connectionString.IsNotNullOrEmpty() )
                return _connectionString;

#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
            var css = new Dahl.Data.Common.ConnectionStringSettings( connectionStringName );
            _connectionString = css.ConnectionString;
#else
            var cs = ConfigurationManager.ConnectionStrings;
            if ( cs == null )
                throw new NullReferenceException("Connection strings undefined.");

            if ( cs[connectionStringName] == null || cs[connectionStringName].ConnectionString.IsNullOrEmpty() )
                throw new NullReferenceException( $"Connection string [{connectionStringName}] is undefined." );

            _connectionString = cs["App.SqlServer"].ConnectionString;
#endif
            return _connectionString;
        }

        #region CreateParameter Methods -----------------------------------------------------------
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

        public IDbDataParameter CreateParameter( string name, byte value )
        {
            return Database.CreateParameter( name, value, typeof( byte ) );
        }

        public IDbDataParameter CreateParameter( string name, short value )
        {
            return Database.CreateParameter( name, value, typeof( short ) );
        }

        public IDbDataParameter CreateParameter( string name, int value )
        {
            return Database.CreateParameter( name, value, typeof( int ) );
        }

        public IDbDataParameter CreateParameter( string name, long value )
        {
            return Database.CreateParameter( name, value, typeof( long ) );
        }

        public IDbDataParameter CreateParameter( string name, decimal value )
        {
            return Database.CreateParameter( name, value, typeof( decimal ) );
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

        public IDbDataParameter CreateParameter( string name, byte? value )
        {
            return Database.CreateParameter( name, value, typeof( byte? ), true );
        }

        public IDbDataParameter CreateParameter( string name, short? value )
        {
            return Database.CreateParameter( name, value, typeof( short? ), true );
        }

        public IDbDataParameter CreateParameter( string name, int? value )
        {
            return Database.CreateParameter( name, value, typeof( int? ), true );
        }

        public IDbDataParameter CreateParameter( string name, long? value )
        {
            return Database.CreateParameter( name, value, typeof( long? ), true );
        }

        public IDbDataParameter CreateParameter( string name, decimal? value )
        {
            return Database.CreateParameter( name, value, typeof( decimal? ), true );
        }

        public IDbDataParameter CreateParameter( string name, double? value )
        {
            return Database.CreateParameter( name, value, typeof( double? ), true );
        }

        public IDbDataParameter CreateParameter( string name, DateTime? value )
        {
            return Database.CreateParameter( name, value, typeof( DateTime? ), true );
        }

        public IDbDataParameter CreateParameter( string name, Guid? value )
        {
            return Database.CreateParameter( name, value, typeof( Guid? ) );
        }
        #endregion
    }
}
