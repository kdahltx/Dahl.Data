using System;
using System.Data;

namespace Dahl.Data.Common
{
    public abstract class BaseRepository
    {
        private IDatabase _database;
        public IDatabase Database
        {
            get { return _database ?? ( _database = CreateDatabase() ); }
            set { _database = value; }
        }

        protected abstract IDatabase CreateDatabase();

        #region Facade for CreateParameter Methods ------------------------------------------------
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
