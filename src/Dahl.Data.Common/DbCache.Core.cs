using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dahl.Data.Common
{
    public partial class DbCache
    {
        private static readonly IMemoryCache _cache = new MemoryCache( new MemoryCacheOptions() );
        private static readonly object _cacheLock = new object();
        private const int _defaultMinutes = 10;

        //-----------------------------------------------------------------------------------------
        public static bool UseSlidingExpiration { get; set; } = true;

        //-----------------------------------------------------------------------------------------
        protected static MemoryCacheEntryOptions GetCacheEntryOptions( int minutes )
        {
            var options = new MemoryCacheEntryOptions();
            if ( UseSlidingExpiration )
                options.SlidingExpiration = new TimeSpan( 0, minutes, 0 );
            else
                options.AbsoluteExpiration = new DateTimeOffset( DateTime.Now ).AddMinutes( minutes );

            return options;
        }

        //-----------------------------------------------------------------------------------------
        protected static ICacheEntry CreateCacheItem( string key, object value, int minutes = _defaultMinutes )
        {
            var item = _cache.CreateEntry( key );
            item.Value = value;
            item.SetOptions( GetCacheEntryOptions( minutes ) );

            return item;
        }

        //-----------------------------------------------------------------------------------------
        public static TResult Get<TResult>( string key, Func<TResult> loadMethod, bool reload, int minutes = _defaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result;
            lock ( _cacheLock )
            {
                if ( reload )
                    Remove( key );

                result = Get<TResult>( key );
                if ( result == null )
                {
                    result = loadMethod();
                    if ( result != null )
                    {
                        ICacheEntry cacheItem = CreateCacheItem( key, result, minutes );
                        _cache.Set( key, cacheItem );
                    }
                }
            }

            return result;
        }

        //-----------------------------------------------------------------------------------------
        public static TResult Get<TResult>( string key, Func<TResult> loadMethod, int minutes = _defaultMinutes ) where TResult : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            TResult result = Get<TResult>( key );
            if ( result == null )
            {
                lock ( _cacheLock )
                {
                    result = Get<TResult>( key );
                    if ( result == null )
                    {
                        result = loadMethod();
                        if ( result != null )
                        {
                            ICacheEntry cacheItem = CreateCacheItem( key, result, minutes );
                            _cache.Set( key, cacheItem );
                        }
                    }
                }
            }

            return result;
        }

        //-----------------------------------------------------------------------------------------
        public static TResult Get<T, TResult>( string key, Func<T, TResult> loadMethod, T param, int minutes = _defaultMinutes ) where TResult : class
        {
            return Get( key, () => loadMethod( param ), minutes );
        }

        //-----------------------------------------------------------------------------------------
        public static T Get<T>( string key ) where T : class
        {
            if ( string.IsNullOrEmpty( key ) )
                return null;

            try
            {
                lock ( _cacheLock )
                {
                    var cacheItem = (ICacheEntry)_cache.Get( key );
                    if ( cacheItem != null )
                        return (T)cacheItem.Value;
                }
            }
            catch ( Exception e )
            {
                Trace.WriteLine( $"Exception: {e.Message}" );
            }

            return null;
        }

        //-----------------------------------------------------------------------------------------
        public static void RefreshItem<T>( string key, T newItem, Func<List<T>, T, List<T>> refreshMethod, int minutes = _defaultMinutes ) where T : class
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock ( _cacheLock )
                {
                    var cacheItem = (ICacheEntry)_cache.Get( key );
                    if ( cacheItem != null )
                    {
                        List<T> list = _cache.Get( key ) as List<T>;
                        if ( list != null )
                        {
                            _cache.Remove( key );
                            List<T> result = refreshMethod( list, newItem );
                            if ( result != null )
                            {
                                cacheItem = CreateCacheItem( key, result, minutes );
                                _cache.Set( key, cacheItem );
                            }
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Removes the data from the cache for the given cacheKey.
        /// </summary>
        /// <param name="key"></param>
        public static void Remove( string key )
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock ( _cacheLock )
                {
                    object result;
                    if ( _cache.TryGetValue( key, out result ) )
                        _cache.Remove( key );
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        public static bool Exists( string key )
        {
            if ( !string.IsNullOrEmpty( key ) )
            {
                lock ( _cacheLock )
                {
                    if ( _cache.Get( key ) != null )
                        return true;
                }
            }

            return false;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Clears the entire cache.
        /// </summary>
        //public static void Clear()
        //{
        //    lock ( _cacheLock )
        //    {
        //        List<string> keyList = new List<string>();
        //        List<KeyValuePair<String, Object>> cacheItems = ( from filter in _cache.Get.AsParallel() select filter ).ToList();

        //        foreach ( KeyValuePair<String, Object> item in cacheItems )
        //            keyList.Add( item.Key );

        //        foreach ( string key in keyList )
        //            _cache.Remove( key );
        //    }
        //}

        ////-----------------------------------------------------------------------------------------
        ///// <summary>
        ///// Removes all entries from the cache.
        ///// </summary>
        //public static void RemoveAll()
        //{
        //    lock ( _cacheLock )
        //    {
        //        var cacheItems = from filter in _cache.AsParallel()
        //                         select filter;

        //        foreach ( KeyValuePair<string, object> item in cacheItems )
        //            Remove( item.Key );
        //    }
        //}

        ////-----------------------------------------------------------------------------------------
        //public static List<string> GetKeys()
        //{
        //    List<string> keys = new List<string>();
        //    lock ( _cacheLock )
        //    {
        //        if ( _cache != null )
        //        {
        //            IEnumerable<string> cacheItems = _cache.AsParallel()
        //                                                   .Select( filter => filter.Key );
        //            if ( cacheItems.Any() )
        //                keys = cacheItems.ToList();
        //        }
        //    }

        //    return keys;
        //}

        ////-----------------------------------------------------------------------------------------
        //public static List<string> GetKeys( string regExPattern )
        //{
        //    List<string> keys = new List<string>();
        //    lock ( _cacheLock )
        //    {
        //        if ( _cache != null )
        //        {
        //            IEnumerable<string> cacheItems = _cache.AsParallel()
        //                                                   .Where( filter => Regex.IsMatch( filter.Key, regExPattern ) )
        //                                                   .Select( filter => filter.Key );
        //            if ( cacheItems.Any() )
        //                keys = cacheItems.ToList();
        //        }
        //    }

        //    return keys;
        //}
    }
}
