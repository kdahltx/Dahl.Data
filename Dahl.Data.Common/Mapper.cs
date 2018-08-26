using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Reflection;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    public class Mapper<TEntity> : IMapper<TEntity> where TEntity : class, new()
    {
        //-----------------------------------------------------------------------------------------
        internal StopwatchCollection swList;

        public void InitStopWatches( int numStopWatches )
        {
            swList = new StopwatchCollection();
            swList.Init( numStopWatches );
        }

        //-----------------------------------------------------------------------------------------
        protected void SetFieldOrdinalsInitialized()
        {
            FieldOrdinalsInitialized = true;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// this is the method to override for setting integer values to variable for
        /// the ordinal value for the columns.
        /// </summary>
        public virtual void SetFieldOrdinals( ConcurrentDictionary<string, IFieldInfo> columns )
        {}

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Initialized data column information into a dictionary collection.
        /// </summary>
        /// <param name="dataRecord">DataRecord with column information to be mapped.</param>
        public virtual void InitFieldOrdinals( IDataRecord dataRecord )
        {
            if ( FieldOrdinalsInitialized )
                return;

            for ( int i = 0; i < dataRecord.FieldCount; i++ )
            {
                FieldInfo ci = new FieldInfo
                {
                    Ordinal      = i,
                    Name         = dataRecord.GetName( i ),
                    DataTypeName = dataRecord.GetDataTypeName( i ),
                    FieldType    = dataRecord.GetFieldType( i )
                };

                Columns.TryAdd( ci.Name, ci );
            }

            SetFieldOrdinals( Columns );
            FieldOrdinalsInitialized = true;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Initialized data column information into a dictionary collection.
        /// </summary>
        /// <param name="cols">DataColumnCollection of row columns to be mapped.</param>
        public virtual void InitFieldOrdinals( DataColumnCollection cols )
        {
            if ( FieldOrdinalsInitialized )
                return;

            for ( int i = 0; i < cols.Count; i++ )
            {
                FieldInfo ci = new FieldInfo
                {
                    Ordinal      = i,
                    Name         = cols[i].ColumnName,
                    DataTypeName = cols[i].DataType.Name,
                    FieldType    = cols[i].DataType
                };

                Columns.TryAdd( ci.Name, ci );
            }

            SetFieldOrdinals( Columns );
            FieldOrdinalsInitialized = true;
        }

        //-----------------------------------------------------------------------------------------
        protected IDataRecord dr;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Code to move data from DataRecord into object properties
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <returns></returns>
        public virtual TEntity Map( IDataRecord dataRecord )
        {
            if ( !FieldOrdinalsInitialized )
                InitFieldOrdinals( dataRecord );

            object[] values = new object[dataRecord.FieldCount];
            dataRecord.GetValues( values );

            return Map( values );
        }

        //-----------------------------------------------------------------------------------------
        public virtual TEntity Map( DataRow dataRecord )
        {
            if ( !FieldOrdinalsInitialized )
                InitFieldOrdinals( dataRecord.Table.Columns );

            return Map( dataRecord.ItemArray );
        }

        //-----------------------------------------------------------------------------------------
        public virtual List<TEntity> Map( DataSet ds )
        {
            throw new NotImplementedException( "List<TEntity> Mapper.Map( DataSet ds )" );
        }

        //-----------------------------------------------------------------------------------------
        // this is the default method for mapping column values into properties of an object.
        // for better performance override this method and use integer variables to hold the
        // the ordinal value of the field.
        private readonly List<IPropertyAccessor> _accessorsList = typeof( TEntity ).GetAccessorList()
                                                                                   .ToList();

        public virtual TEntity Map( object[] values )
        {
            TEntity entity = new TEntity();
            Type    t      = entity.GetType();

            foreach ( var ci in Columns.Values )
            {
                object o = values[ci.Ordinal];
                if ( o != DBNull.Value )
                {
                    PropertyInfo p = t.GetProperty( ci.Name );
                    if ( p != null && p.CanWrite )
                        p.SetValue( entity, o );

                    var accessor = _accessorsList.Find( x => x.Ordinal == ci.Ordinal );
                    accessor.SetValue( entity, o );
                }
            }

            return entity;
        }

        //-----------------------------------------------------------------------------------------
        protected bool                                     FieldOrdinalsInitialized;
        protected ConcurrentDictionary<string, IFieldInfo> Columns = new ConcurrentDictionary<string, IFieldInfo>();

        //-----------------------------------------------------------------------------------------
        public class FieldInfo : IFieldInfo
        {
            public string Name         { get; set; }
            public Type   FieldType    { get; set; }
            public string DataTypeName { get; set; }
            public int    Ordinal      { get; set; }
        }
    }
}
