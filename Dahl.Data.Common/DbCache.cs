#if !NETCOREAPP2_0
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Dahl.Data.Common
{
    public partial class DbCache
    {
        static private readonly ObjectCache _Cache = MemoryCache.Default;
        static private object _CacheLock = new object();
        static private bool   _useSlidingExpiration = true;
        private const  int    _DefaultMinutes = 10; // 10 minutes

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        static public bool UseSlidingExpiration
        {
            get { return _useSlidingExpiration; }
            set { _useSlidingExpiration = value; }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        static protected CacheItemPolicy GetCachePolicy( int minutes )
        {
            CacheItemPolicy cachePolicy = new CacheItemPolicy();
            if ( _useSlidingExpiration )
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
        /// <returns></returns>
        static public bool Add<TResult>( string key, TResult item, int minutes = _DefaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return false;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock ( _CacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        return _Cache.Add( key, item, GetCachePolicy( minutes <= 0 ? _DefaultMinutes : minutes ) );
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
        static public TResult Get<TResult>( string key, Func<TResult> loadMethod, bool reload, int minutes = _DefaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = null;
            lock ( _CacheLock )
            {
                if ( reload )
                    Remove( key );

                result = Get<TResult>( key );
                if ( result == null )
                {
                    result = Get<TResult>( key );
                    result = loadMethod();
                    if ( result != null )
                        _Cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _DefaultMinutes : minutes ) );
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
        static public TResult Get<TResult>( string key, Func<TResult> loadMethod, int minutes = _DefaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock ( _CacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        result = loadMethod();
                        if ( result != null )
                            _Cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _DefaultMinutes : minutes ) );
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
        /// <param name="key"></param>
        /// <param name="loadMethod"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        static public TResult Get<T, TResult>( string key, Func<T, TResult> loadMethod, T param, int minutes = _DefaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock ( _CacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        result = loadMethod( param );
                        if ( result != null )
                            _Cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _DefaultMinutes : minutes ) );
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
        /// <param name="key"></param>
        /// <param name="loadMethod"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        static public TResult Get<T1, T2, TResult>( string key, Func<T1, T2, TResult> loadMethod, T1 param1, T2 param2, int minutes = _DefaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock ( _CacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        result = loadMethod( param1, param2 );
                        if ( result != null )
                            _Cache.Add( key, result, GetCachePolicy( minutes <= 0 ? _DefaultMinutes : minutes ) );
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
        static public TResult Get<TResult>( Func<TResult> loadMethod, int minutes = _DefaultMinutes ) where TResult : class
        {
            System.Reflection.MethodBase method = new StackTrace().GetFrame( 1 ).GetMethod();
            string key = string.Format( "{0}.{1}", method.DeclaringType.FullName, method.Name );

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
        static public TResult Get<T, TResult>( Func<T, TResult> loadMethod, T param, int minutes = _DefaultMinutes ) where TResult : class
        {
            System.Reflection.MethodBase method = new StackTrace().GetFrame( 1 ).GetMethod();
            string key = string.Format( "{0}.1:{2}", method.DeclaringType.FullName, method.Name, param );

            return Get( new Func<TResult>( () => loadMethod( param ) ), minutes );
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        static public T Get<T>( string key ) where T : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            try
            {
                return (T)_Cache[key];
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
        static public void RefreshItem<T>( string key, T newItem, Func<List<T>, T, List<T>> refreshMethod ) where T : class
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock ( _CacheLock )
                {
                    if ( _Cache[key] != null )
                    {
                        List<T> list = _Cache[key] as List<T>;
                        if ( list != null )
                        {
                            _Cache.Remove( key );
                            List<T> result = refreshMethod( list, newItem );
                            _Cache.Add( key, result, GetCachePolicy( _DefaultMinutes ) );
                        }
                    }
                }
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Removes the data from the cache for the given cacheKey.
        /// </summary>
        /// <param name="key"></param>
        static public void Remove( string key )
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock ( _CacheLock )
                {
                    if ( _Cache[key] != null )
                        _Cache.Remove( key );
                }
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool Exists( string key )
        {
            bool result = false;
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock ( _CacheLock )
                {
                    if ( _Cache[key] != null )
                        return true;
                }
            }

            return result;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Clears the entire cache.
        /// </summary>
        static public void Clear()
        {
            lock ( _CacheLock )
            {
                List<string> keyList = new List<string>();
                List<KeyValuePair<String, Object>> cacheItems = ( from filter in _Cache.AsParallel() select filter ).ToList();

                foreach ( KeyValuePair<String, Object> item in cacheItems )
                    keyList.Add( item.Key );

                foreach ( string key in keyList )
                    _Cache.Remove( key );
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        static public void RemoveAll()
        {
            lock ( _CacheLock )
            {
                var cacheItems = from filter in _Cache.AsParallel()
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
        static public List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            IEnumerable<string> cacheItems = from filter in _Cache.AsParallel()
                                             select filter.Key;

            if ( cacheItems.Count() > 0 )
                keys = cacheItems.ToList();

            return keys;

        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regExPattern"></param>
        /// <returns></returns>
        static public List<string> GetKeys( string regExPattern )
        {
            List<string> keys = new List<string>();
            IEnumerable<string> cacheItems = from filter in _Cache.AsParallel()
                                             where Regex.IsMatch( filter.Key, regExPattern )
                                             select filter.Key;

            if ( cacheItems.Count() > 0 )
                keys = cacheItems.ToList();

            return keys;

        }
    }
}
#endif
