using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    // DbDataReader,
    public class EntityReader<TEntity> : DbDataReader, IEntityReader
    {
        private IEnumerator<TEntity> _entityEnumerator;

        public override int  RecordsAffected { get { return -1; } }
        public override int  Depth           { get { return 1; } }
        public override int  FieldCount      { get { return _accessorsList.Length; } }
        public override bool IsClosed        { get { return _entityEnumerator == null; } }
        public override bool HasRows         { get { return true; } }

        // 2 dictionaries to lookup by either property ordinal or property name
        // ordinal is the index into accessorsList array for property.
        private readonly Dictionary<int, string> _ordinalLookup = new Dictionary<int, string>();
        private readonly Dictionary<string, int> _nameLookup    = new Dictionary<string, int>();
        private readonly IPropertyAccessor[]     _accessorsList = typeof( TEntity ).GetAccessorList();

        //-----------------------------------------------------------------------------------------
        public EntityReader( IEnumerable<TEntity> list )
        {
            if ( list == null )
                throw new Exception( "public EntityReader(IEnumerable<TEntity> list), list is null." );

            if ( _accessorsList == null || _accessorsList.Length == 0 )
                throw new Exception( "public EntityReader(IEnumerable<TEntity> list), no properties in TEntity." );

            _nameLookup       = _accessorsList.ToDictionary( x => x.Name,    x => x.Ordinal );
            _ordinalLookup    = _accessorsList.ToDictionary( x => x.Ordinal, x => x.Name );
            _entityEnumerator = list.GetEnumerator();
        }

        //-----------------------------------------------------------------------------------------
        public override object this[ string name ]
        {
            get
            {
                if ( _nameLookup.TryGetValue( name, out int ordinal ) )
                    return _accessorsList[ordinal].GetValue( _entityEnumerator.Current );

                return null;
            }
        }

        //-----------------------------------------------------------------------------------------
        public override object this[ int ordinal ]
        {
            get
            {
                if ( _ordinalLookup.TryGetValue( ordinal, out string name ) )
                    return _accessorsList[ordinal].GetValue( _entityEnumerator.Current );

                return null;
            }
        }

        public override bool Read()
        {
            if ( _entityEnumerator == null )
                throw new ObjectDisposedException( "EntityReader" );

            return _entityEnumerator.MoveNext();
        }

        public override bool NextResult()
        {
            return false;
        }

        public override void Close()
        {
            Trace.WriteLine( $"{GetType().FullName}.Close()" );
            Dispose();
        }

        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( _entityEnumerator != null )
                {
                    _entityEnumerator.Dispose();
                    _entityEnumerator = null;
                }
            }
        }

        public override string GetName( int i )
        {
            return _accessorsList[i].Name;
        }

        public override string GetDataTypeName( int i )
        {
            return _accessorsList[i].PropertyInfo.PropertyType.Name;
        }

        public override Type GetFieldType( int i )
        {
            return _accessorsList[i].PropertyInfo.PropertyType;
        }

        public override object GetValue( int i )
        {
            if ( _entityEnumerator == null )
                throw new ObjectDisposedException( "EntityReader" );

            return _accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override int GetValues( object[] values )
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal( string name )
        {
            if ( _nameLookup.TryGetValue( name, out var ordinal ) )
                return ordinal;

            throw new InvalidOperationException( "Unknown parameter name " + name );
        }

        public override bool GetBoolean( int i )
        {
            return (bool)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override byte GetByte( int i )
        {
            return (byte)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override long GetBytes( int i, long fieldOffset, byte[] buffer, int bufferoffset, int length )
        {
            throw new NotImplementedException();
        }

        public override char GetChar( int i )
        {
            return (char)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override long GetChars( int i, long fieldoffset, char[] buffer, int bufferoffset, int length )
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid( int i )
        {
            return (Guid)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override short GetInt16( int i )
        {
            return (short)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override int GetInt32( int i )
        {
            return (Int32)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override long GetInt64( int i )
        {
            return (Int64)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override float GetFloat( int i )
        {
            return (float)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override double GetDouble( int i )
        {
            return (double)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override string GetString( int i )
        {
            return (string)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override decimal GetDecimal( int i )
        {
            return (decimal)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override DateTime GetDateTime( int i )
        {
            return (DateTime)_accessorsList[i].GetValue( _entityEnumerator.Current );
        }

        public override bool IsDBNull( int i )
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
