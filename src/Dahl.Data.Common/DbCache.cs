#if !NETCOREAPP3_1 && !NET5_0
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Dahl.Data.Common
{
    public class DbCache
    {
        private static readonly ObjectCache _cache                = MemoryCache.Default;
        private static readonly object      _cacheLock            = new object();
        private const           int         _defaultMinutes       = 10; // 10 minutes

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public static bool UseSlidingExpiration { get; set; } = true;

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        protected static CacheItemPolicy GetCachePolicy( int minutes )
        {
            CacheItemPolicy cachePolicy = new CacheItemPolicy();
            if ( UseSlidingExpiration )
                cachePolicy.SlidingExpiration = new TimeSpan( 0, minutes, 0 );
            else
                cachePolicy.AbsoluteExpiration = new DateTimeOffset( DateTime.Now ).AddMinutes( minutes );

            return cachePolicy;
        }

        /// <summary>
        /// If an item with the key does not exist in the cache, then the item is added to the cache.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static bool Add<TResult>( string key, TResult item, int minutes = _defaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return false;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock( _cacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        return _cache.Add( key, item, GetCachePolicy( minutes <= 0 ? _defaultMinutes : minutes ) );
                    }
                }
            }

            return false;
        }

        ///------------------------------------------------------------------------------------------
        /// <summary>
        /// Get result from Cache
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="loadMethod"></param>
        /// <param name="reload"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TResult Get<TResult>( string key, Func<TResult> loadMethod, bool reload, int minutes = _defaultMinutes )
            where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result;
            lock( _cacheLock )
            {
                if ( reload )
                    Remove( key );

                result = Get<TResult>( key );
                if ( result == null )
                {
                    Get<TResult>( key );
                    result = loadMethod();
                    if ( result != null )
                        _cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _defaultMinutes : minutes ) );
                }
            }

            return result;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="loadMethod"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TResult Get<TResult>( string key, Func<TResult> loadMethod, int minutes = _defaultMinutes )
            where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock( _cacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        result = loadMethod();
                        if ( result != null )
                            _cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _defaultMinutes : minutes ) );
                    }
                }
            }

            return result;
        }

        /// ----------------------------------------------------------------------------------------
        ///  <summary>
        ///  
        ///  </summary>
        ///  <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        ///  <param name="loadMethod"></param>
        /// <param name="param"></param>
        /// <param name="minutes"></param>
        ///  <returns></returns>
        public static TResult Get<T, TResult>( string key, Func<T, TResult> loadMethod, T param, int minutes = _defaultMinutes )
            where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock( _cacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        result = loadMethod( param );
                        if ( result != null )
                            _cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _defaultMinutes : minutes ) );
                    }
                }
            }

            return result;
        }

        /// ----------------------------------------------------------------------------------------
        ///  <summary>
        ///  
        ///  </summary>
        ///  <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="key"></param>
        ///  <param name="loadMethod"></param>
        /// <param name="param2"></param>
        /// <param name="minutes"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public static TResult Get<T1, T2, TResult>( string key, Func<T1, T2, TResult> loadMethod, T1 param1, T2 param2,
                                                    int    minutes = _defaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock( _cacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        result = loadMethod( param1, param2 );
                        if ( result != null )
                            _cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _defaultMinutes : minutes ) );
                    }
                }
            }

            return result;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="loadMethod"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TResult Get<TResult>( Func<TResult> loadMethod, int minutes = _defaultMinutes ) where TResult : class
        {
            System.Reflection.MethodBase method = new StackTrace().GetFrame( 1 ).GetMethod();
            string                       key    = $"{method?.DeclaringType?.FullName}.{method?.Name}";

            return Get( key, loadMethod, minutes );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="loadMethod"></param>
        /// <param name="param"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TResult Get<T, TResult>( Func<T, TResult> loadMethod, T param, int minutes = _defaultMinutes )
            where TResult : class
        {
            System.Reflection.MethodBase method = new StackTrace().GetFrame( 1 ).GetMethod();

            if ( method == null )
                return null;

            string key = $"{method.DeclaringType?.FullName}.{method.Name}:{param}";

            return Get( key, () => loadMethod( param ), minutes );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>( string key ) where T : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            try
            {
                lock( _cacheLock )
                {
                    return (T)_cache[key];
                }
            }
            catch
            {
                return null;
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="newItem"></param>
        /// <param name="refreshMethod"></param>
        public static void RefreshItem<T>( string key, T newItem, Func<List<T>, T, List<T>> refreshMethod ) where T : class
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock( _cacheLock )
                {
                    if ( _cache[key] != null && _cache[key] is List<T> list )
                    {
                        _cache.Remove( key );
                        List<T> result = refreshMethod( list, newItem );
                        _cache.Add( key, result, GetCachePolicy( _defaultMinutes ) );
                    }
                }
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Removes the data from the cache for the given cacheKey.
        /// </summary>
        /// <param name="key"></param>
        public static void Remove( string key )
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock( _cacheLock )
                {
                    if ( _cache[key] != null )
                        _cache.Remove( key );
                }
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists( string key )
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock( _cacheLock )
                {
                    if ( _cache[key] != null )
                        return true;
                }
            }

            return false;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Clears the entire cache.
        /// </summary>
        public static void Clear()
        {
            lock( _cacheLock )
            {
                List<string>                       keyList    = new List<string>();
                List<KeyValuePair<String, Object>> cacheItems = ( from filter in _cache.AsParallel() select filter ).ToList();

                foreach ( KeyValuePair<String, Object> item in cacheItems )
                    keyList.Add( item.Key );

                foreach ( string key in keyList )
                    _cache.Remove( key );
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        public static void RemoveAll()
        {
            lock( _cacheLock )
            {
                var cacheItems = from filter in _cache.AsParallel()
                                 select filter;

                foreach ( KeyValuePair<String, Object> item in cacheItems )
                    Remove( item.Key );
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            if ( _cache != null )
            {
                lock( _cacheLock )
                {
                    IEnumerable<string> cacheItems = from filter in _cache.AsParallel()
                                                     select filter.Key;

                    var enumerable = cacheItems.ToList();
                    if ( enumerable.Any() )
                        keys = enumerable.ToList();
                }
            }

            return keys;

        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regExPattern"></param>
        /// <returns></returns>
        public static List<string> GetKeys( string regExPattern )
        {
            List<string> keys = new List<string>();
            if ( _cache != null )
            {
                lock( _cacheLock )
                {
                    List<string> cacheItems = ( from filter in _cache.AsParallel()
                                                where Regex.IsMatch( filter.Key, regExPattern )
                                                select filter.Key ).ToList();

                    if ( cacheItems.Any() )
                        keys = cacheItems;
                }
            }

            return keys;

        }
    }
}
#endif
