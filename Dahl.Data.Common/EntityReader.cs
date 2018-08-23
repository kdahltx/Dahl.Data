using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    // DbDataReader, 
    public class EntityReader<TEntity> : DbDataReader, IEntityReader
    {
        private IEnumerator<TEntity> entityEnumerator;

        public override int RecordsAffected { get { return -1; } }
        public override int Depth { get { return 1; } }
        public override int FieldCount { get { return accessorsList.Length; } }
        public override bool IsClosed { get { return entityEnumerator == null; } }
        public override bool HasRows { get { return true; } }

        // 2 dictionaries to lookup by either property ordinal or property name
        // ordinal is the index into accessorsList array for property.
        private Dictionary<int, string> ordinalLookup = new Dictionary<int, string>();
        private Dictionary<string, int> nameLookup = new Dictionary<string, int>();

        private readonly IPropertyAccessor[] accessorsList = typeof( TEntity ).GetAccessorList();

        //-----------------------------------------------------------------------------------------
        public EntityReader(IEnumerable<TEntity> list)
        {
            if ( list == null )
                throw new Exception( "public EntityReader(IEnumerable<TEntity> list), list is null." );

            if ( accessorsList == null || accessorsList.Length == 0 )
                throw new Exception( "public EntityReader(IEnumerable<TEntity> list), no properties in TEntity." );

            nameLookup    = accessorsList.ToDictionary( x => x.Name, x => x.Ordinal );
            ordinalLookup = accessorsList.ToDictionary( x => x.Ordinal, x => x.Name );
            entityEnumerator = list.GetEnumerator();
        }

        //-----------------------------------------------------------------------------------------
        public override object this[string name]
        {
            get
            {
                if ( nameLookup.TryGetValue( name, out int ordinal ) )
                    return accessorsList[ordinal].GetValue( entityEnumerator.Current );

                return null;
            }
        }

        //-----------------------------------------------------------------------------------------
        public override object this[int ordinal]
        {
            get
            {
                if ( ordinalLookup.TryGetValue( ordinal, out string name ) )
                    return accessorsList[ordinal].GetValue( entityEnumerator.Current );

                return null;
            }
        }

        public override bool Read()
        {
            if ( entityEnumerator == null )
                throw new ObjectDisposedException( "EntityReader" );

            return entityEnumerator.MoveNext();
        }

        public override bool NextResult()
        {
            return false;
        }

        public override void Close()
        {
            Trace.WriteLine($"{GetType().FullName}.Close()");
            Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if ( disposing)
            {
                if (entityEnumerator != null)
                {
                    entityEnumerator.Dispose();
                    entityEnumerator = null;
                }
            }
        }

        public override string GetName(int i)
        {
            return accessorsList[i].Name;
        }

        public override string GetDataTypeName(int i)
        {
            return accessorsList[i].PropertyInfo.PropertyType.Name;
        }

        public override Type GetFieldType(int i)
        {
            return accessorsList[i].PropertyInfo.PropertyType;
        }

        public override object GetValue(int i)
        {
            if (entityEnumerator == null)
                throw new ObjectDisposedException("EntityReader");

            return accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal(string name)
        {
            int ordinal;
            if (nameLookup.TryGetValue(name, out ordinal))
                return ordinal;

            throw new InvalidOperationException("Unknown parameter name " + name);
        }

        public override bool GetBoolean(int i)
        {
            return (bool)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override byte GetByte(int i)
        {
            return (byte)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int i)
        {
            return (char)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int i)
        {
            return (Guid)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override short GetInt16(int i)
        {
            return (short)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override int GetInt32(int i)
        {
            return (Int32)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override long GetInt64(int i)
        {
            return (Int64)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override float GetFloat(int i)
        {
            return (float)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override double GetDouble(int i)
        {
            return (double)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override string GetString(int i)
        {
            return (string)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override decimal GetDecimal(int i)
        {
            return (decimal)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override DateTime GetDateTime(int i)
        {
            return (DateTime)accessorsList[i].GetValue( entityEnumerator.Current );
        }

        public override bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
