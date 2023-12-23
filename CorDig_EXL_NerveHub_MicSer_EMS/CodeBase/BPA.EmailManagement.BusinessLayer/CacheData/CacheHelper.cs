using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    /// <summary>
    /// CacheHelper
    /// </summary>
    public class CacheHelper
    {
        #region Appfabric cache
        private string _cacheName;
        private static DataCache _cache;
        DataCacheLockHandle handle;
        #endregion 

        #region Memory cache
        public static bool isAppfabricDown = false;
        public static DateTime TimeCheckAppbric = DateTime.Now;

        public static ObjectCache _memorycache = MemoryCache.Default;
        private CacheItemPolicy policy = null;
        private CacheEntryRemovedCallback callback = null;

        private void setMemorycachePolicy()
        {
            setMemorycachePolicy(new TimeSpan(8, 0, 0));
        }
        private void setMemorycachePolicy(TimeSpan ts)
        {
            callback = new CacheEntryRemovedCallback(this.MyCachedItemRemovedCallback);
            policy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                AbsoluteExpiration = DateTimeOffset.Now.AddTicks(ts.Ticks),
                RemovedCallback = callback
            };
        }
        private void MyCachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            // Log these values from arguments list 
            String strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), " | Key-Name: ", arguments.CacheItem.Key, " | Value-Object: ", arguments.CacheItem.Value.ToString());
        }
        #endregion



        /// <summary>
        /// CacheHelper
        /// </summary>
        public CacheHelper(string CacheName)
        {
            try
            {
                if (!isAppfabricDown)
                {
                    _cache = CreateCache(CacheName);
                }
                else if (isAppfabricDown && DateTime.Now > TimeCheckAppbric)
                {
                    _cache = CreateCache(CacheName);
                    isAppfabricDown = false;

                    //Remove memorycache

                }
            }
            catch
            {
                _cache = null;
                try
                {
                    //BPA.ExceptionHandler.Logging.EHLogger.WriteLog("Appfabric is Down", ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);//issue
                }
                catch { }

                TimeCheckAppbric = DateTime.Now.AddMinutes(30);
                isAppfabricDown = true;
            }
        }

        /// <summary>
        /// CreateCache
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        private static DataCache CreateCache(string cacheName)
        {
            DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration();
            configuration.DataCacheServiceAccountType = DataCacheServiceAccountType.DomainAccount;
            DataCacheFactory factory = new DataCacheFactory(configuration);
            DataCache cache = factory.GetCache(cacheName);
            // cache.CreateRegion(CacheRegionName);

            return cache;
        }

        /// <summary>
        /// GetFromCache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetFromCache(string key)
        {
            object obj = null;
            try
            {
                obj = _cache != null ? _cache.Get(key) : _memorycache[key] as Object;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return obj;
        }
        //public object GetFromCache(string key, out object cacheHandler)
        //{
        //    object obj = null;
        //    try
        //    {
        //        if (_cache != null)
        //            obj = _cache.GetAndLock(key, TimeSpan.FromMilliseconds(10), out handle, true);
        //            cacheHandler = handle;

        //    }
        //    catch (Exception ex)
        //    {
        //        handle=null;
        //        throw ex;
        //    }
        //    return obj;
        //}
        //public void AddToCache(string vsalue, object obj,TimeSpan tspan, object cacheHandler)
        //{
        //    DataCacheLockHandle dc = (DataCacheLockHandle)cacheHandler;
        //    try
        //    {
        //        if (_cache != null)
        //            _cache.PutAndUnlock(vsalue, obj, dc, tspan);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dc = null;
        //    }
        //}

        // update an item

        //public void AddToCache(string key, object obj)
        //{
        //    try
        //    {
        //        if (_cache != null)
        //            _cache.Put(key, obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void RemoveFromCache(string key)
        {
            try
            {
                if (_cache != null)
                { _cache.Remove(key); }
                else if (_memorycache.Contains(key))
                {
                    _memorycache.Remove(key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddToCache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="tspan"></param>
        public void AddToCache(string key, object obj, TimeSpan tspan)
        {
            try
            {
                if (_cache != null)
                {
                    _cache.Put(key, obj, tspan);
                }
                else
                {
                    setMemorycachePolicy(tspan);
                    _memorycache.Set(key, obj, policy);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ResetObjectTimeoutWrapper
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newTimeout"></param>
        public void ResetObjectTimeoutWrapper(string key, TimeSpan newTimeout)
        {
            //TracingLibrary.TraceEvents.AddEvent("Inside ResetObjectTimeoutWrapper");
            try
            {
                if (_cache != null)
                    _cache.ResetObjectTimeout(key, newTimeout);
            }
            catch (Exception ex)
            {
                throw ex;
                //write to logger ("Exception occured in cache", e.InnerException);
            }
        }


    }
}
