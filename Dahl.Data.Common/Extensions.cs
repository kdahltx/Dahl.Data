using System;
using System.Data;

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
        public static string AsString(this IDataRecord dr, string fieldName, string defaultValue = default(string))
        {
            return dr.IsDbNull(fieldName) ? defaultValue : (string)dr[fieldName];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool IsDbNull(this IDataRecord dr, string fieldName)
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
        public static short GetSafeInt16(this IDataRecord dr, int ordinal, short defaultValue = default(short))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt16(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short? GetSafeInt16Nullable(this IDataRecord dr, int ordinal, short? defaultValue = default(short?))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt16(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetSafeInt32(this IDataRecord dr, int ordinal, int defaultValue = default(int))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt32(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetSafeInt32Nullable(this IDataRecord dr, int ordinal, int? defaultValue = default(int?))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt32(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetSafeDateTime(this IDataRecord dr, int ordinal, DateTime defaultValue = default(DateTime))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetDateTime(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetSafeDateTimeNullable(this IDataRecord dr, int ordinal, DateTime? defaultValue = default(DateTime?))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetDateTime(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSafeString(this IDataRecord dr, int ordinal, string defaultValue = default(string))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetString(ordinal);
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
        public static T GetSafeValueOrDefault<T>(this IDataRecord dr, int ordinal, T defaultValue = default(T))
        {
            return (T)(dr.IsDbNull(ordinal) ? defaultValue : dr.GetValue(ordinal));
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
        public static T GetSafeValueOrDefault<T>(this IDataRecord dr, string fieldName, T defaultValue = default(T))
        {
            return (T)(dr.IsDbNull(fieldName) ? defaultValue : dr[fieldName]);
        }

        public static bool IsDbNull(this IDataRecord dr, int ordinal)
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
        public static string AsString(this DataRow dr, string fieldName, string defaultValue = default(string))
        {
            return dr.IsDbNull(fieldName) ? defaultValue : (string)dr[fieldName];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool IsDbNull(this DataRow dr, string fieldName)
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
        public static short GetInt16(this DataRow dr, int ordinal, short defaultValue = default(short))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : (short)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt32(this DataRow dr, int ordinal, int defaultValue = default(int))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : (int)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this DataRow dr, int ordinal, DateTime defaultValue = default(DateTime))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : (DateTime)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AsString(this DataRow dr, int ordinal, string defaultValue = default(string))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : (string)dr[ordinal];
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short GetSafeInt16(this DataRow dr, int ordinal, short defaultValue = default(short))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt16(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short? GetSafeInt16Nullable(this DataRow dr, int ordinal, short? defaultValue = default(short?))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt16(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetSafeInt32(this DataRow dr, int ordinal, int defaultValue = default(int))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt32(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetSafeInt32Nullable(this DataRow dr, int ordinal, int? defaultValue = default(int?))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.GetInt32(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetSafeDateTime(this DataRow dr, int ordinal, DateTime defaultValue = default(DateTime))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.AsDateTime(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetSafeDateTimeNullable(this DataRow dr, int ordinal, DateTime? defaultValue = default(DateTime?))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.AsDateTime(ordinal);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSafeString(this DataRow dr, int ordinal, string defaultValue = default(string))
        {
            return dr.IsDbNull(ordinal) ? defaultValue : dr.AsString(ordinal);
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
        public static T GetSafeValueOrDefault<T>(this DataRow dr, int ordinal, T defaultValue = default(T))
        {
            return (T)(dr.IsDbNull(ordinal) ? defaultValue : dr[ordinal]);
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
        public static T GetSafeValueOrDefault<T>(this DataRow dr, string fieldName, T defaultValue = default(T))
        {
            return (T)(dr.IsDbNull(fieldName) ? defaultValue : dr[fieldName]);
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static bool IsDbNull(this DataRow dr, int ordinal)
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
        public static T As<T>(this object obj, T defaultValue = default(T))
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
        public static string AsString(this object obj, string defaultValue = default(string))
        {
            return (obj == DBNull.Value) ? defaultValue : (string)obj;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long AsLong(this object obj, long defaultValue = default(long))
        {
            return (obj != null) ? (long)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int AsInt(this object obj, int defaultValue = default(int))
        {
            return (obj != null) ? (int)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short AsShort(this object obj, short defaultValue = default(short))
        {
            return (obj != null) ? (short)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static byte AsByte(this object obj, byte defaultValue = default(byte))
        {
            return (obj != null) ? (byte)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this object obj, DateTime defaultValue = default(DateTime))
        {
            return (obj != null) ? (DateTime)obj : defaultValue;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short? AsShortNullable(this object obj, short? defaultValue = default(short?))
        {
            return obj != DBNull.Value ? (short?)obj : null;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? AsIntNullable(this object obj, int defaultValue = default(int))
        {
            return obj != DBNull.Value ? (int?)obj : null;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? AsDateTimeNullable(this object obj, DateTime? defaultValue = default(DateTime?))
        {
            return obj != DBNull.Value ? (DateTime?)obj : null;
        }
        #endregion
    }
}
