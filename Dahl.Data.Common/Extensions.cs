using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    public static class Extensions
    {
        #region IDataRecord -----------------------------------------------------------------------
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AsString( this IDataRecord dr, string fieldName, string defaultValue = default )
        {
            return dr.IsDbNull( fieldName ) ? defaultValue : (string)dr[fieldName];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool IsDbNull( this IDataRecord dr, string fieldName )
        {
            return dr[fieldName] == DBNull.Value;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short GetSafeInt16( this IDataRecord dr, int ordinal, short defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt16( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short? GetSafeInt16Nullable( this IDataRecord dr, int ordinal, short? defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt16( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetSafeInt32( this IDataRecord dr, int ordinal, int defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt32( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetSafeInt32Nullable( this IDataRecord dr, int ordinal, int? defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt32( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetSafeDateTime( this IDataRecord dr, int ordinal, DateTime defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetDateTime( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetSafeDateTimeNullable( this IDataRecord dr, int ordinal,
                                                         DateTime?        defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetDateTime( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSafeString( this IDataRecord dr, int ordinal, string defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetString( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the value for the data column by ordinal value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetSafeValueOrDefault<T>( this IDataRecord dr, int ordinal, T defaultValue = default )
        {
            return (T)( dr.IsDbNull( ordinal ) ? defaultValue : dr.GetValue( ordinal ) );
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the value for the data column by field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetSafeValueOrDefault<T>( this IDataRecord dr, string fieldName, T defaultValue = default )
        {
            return (T)( dr.IsDbNull( fieldName ) ? defaultValue : dr[fieldName] );
        }

        public static bool IsDbNull( this IDataRecord dr, int ordinal )
        {
            return dr[ordinal] == DBNull.Value;
        }
        #endregion

        #region DataRow ---------------------------------------------------------------------------
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AsString( this DataRow dr, string fieldName, string defaultValue = default )
        {
            return dr.IsDbNull( fieldName ) ? defaultValue : (string)dr[fieldName];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool IsDbNull( this DataRow dr, string fieldName )
        {
            return dr[fieldName] == DBNull.Value;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short GetInt16( this DataRow dr, int ordinal, short defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : (short)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt32( this DataRow dr, int ordinal, int defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : (int)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime AsDateTime( this DataRow dr, int ordinal, DateTime defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : (DateTime)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AsString( this DataRow dr, int ordinal, string defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : (string)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short GetSafeInt16( this DataRow dr, int ordinal, short defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt16( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short? GetSafeInt16Nullable( this DataRow dr, int ordinal, short? defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt16( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetSafeInt32( this DataRow dr, int ordinal, int defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt32( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetSafeInt32Nullable( this DataRow dr, int ordinal, int? defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.GetInt32( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetSafeDateTime( this DataRow dr, int ordinal, DateTime defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.AsDateTime( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetSafeDateTimeNullable( this DataRow dr, int ordinal,
                                                         DateTime?    defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.AsDateTime( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSafeString( this DataRow dr, int ordinal, string defaultValue = default )
        {
            return dr.IsDbNull( ordinal ) ? defaultValue : dr.AsString( ordinal );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the value for the data column by ordinal value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetSafeValueOrDefault<T>( this DataRow dr, int ordinal, T defaultValue = default )
        {
            return (T)( dr.IsDbNull( ordinal ) ? defaultValue : dr[ordinal] );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the value for the data column by field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetSafeValueOrDefault<T>( this DataRow dr, string fieldName, T defaultValue = default )
        {
            return (T)( dr.IsDbNull( fieldName ) ? defaultValue : dr[fieldName] );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static bool IsDbNull( this DataRow dr, int ordinal )
        {
            return dr[ordinal] == DBNull.Value;
        }
        #endregion

        #region object ----------------------------------------------------------------------------
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T As<T>( this object obj, T defaultValue = default )
        {
            return obj != null ? (T)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AsString( this object obj, string defaultValue = default )
        {
            return ( obj == DBNull.Value ) ? defaultValue : (string)obj;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long AsLong( this object obj, long defaultValue = default )
        {
            return ( obj != null ) ? (long)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int AsInt( this object obj, int defaultValue = default )
        {
            return ( obj != null ) ? (int)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short AsShort( this object obj, short defaultValue = default )
        {
            return ( obj != null ) ? (short)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static byte AsByte( this object obj, byte defaultValue = default )
        {
            return ( obj != null ) ? (byte)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime AsDateTime( this object obj, DateTime defaultValue = default )
        {
            return ( obj != null ) ? (DateTime)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short? AsShortNullable( this object obj, short? defaultValue = default )
        {
            return obj != DBNull.Value ? (short?)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? AsIntNullable( this object obj, int defaultValue = default )
        {
            return obj != DBNull.Value ? (int?)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? AsDateTimeNullable( this object obj, DateTime? defaultValue = default )
        {
            return obj != DBNull.Value ? (DateTime?)obj : defaultValue;
        }
        #endregion

        #region DataTable -------------------------------------------------------------------------
        public static List<TEntity> ToList<TEntity>( this DataTable dt ) where TEntity : class, new()
        {
            var list = new List<TEntity>();
            var accessorsList = typeof( TEntity ).GetAccessorList().ToDictionary( x => x.Ordinal, x => x );
            foreach ( DataRow row in dt.Rows )
            {
                var entity = new TEntity();
                for ( int i = 0; i < accessorsList.Count; i++ )
                {
                    if ( accessorsList.TryGetValue( i, out var p ) )
                        p.SetValue( entity, row[p.Name] );
                }
                list.Add( entity );
            }

            return list;
        }

        /// <summary>
        /// Convert entity list to a DataTable object.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TEntity>( this List<TEntity> list ) where TEntity : class, new()
        {
            DataTable dt = new DataTable( typeof( TEntity).Name );

            var accessorsList = typeof( TEntity ).GetAccessorList().ToDictionary( x => x.Ordinal, x => x );
            for ( int i = 0; i < accessorsList.Count; i++ )
            {
                if ( accessorsList.TryGetValue( i, out var p ) )
                {
                    int  ordinal    = p.Ordinal;
                    Type type       = p.PropertyInfo.GetType();
                    bool isNullable = Nullable.GetUnderlyingType( type ) != null;

                    var dc = new DataColumn
                    {
                        ColumnName  = p.Name,
                        DataType    = p.PropertyInfo.PropertyType,
                        AllowDBNull = isNullable
                    };
                    dt.Columns.Add( dc );
                }
            }

            foreach ( TEntity entity in list )
            {
                var dr = dt.NewRow();
                for ( int i = 0; i < accessorsList.Count; i++ )
                {
                    if ( accessorsList.TryGetValue( i, out var p ) )
                        dr[p.Name] = p.GetValue( entity );
                }

                dt.Rows.Add( dr );
            }

            return dt;
        }
        #endregion
    }
}
