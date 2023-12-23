using Domino;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.Dominos
{
    public static class LotusUtility
    {
        internal static Domino.NotesDatabase ConnectToLotusServer(string lotusServer, string nsfFile,
               string userPas, int campaignId = 0, bool isCallFromConfiguration = false)
        {
            string cacheName = lotusServer + "_" + nsfFile + "_" + userPas + "_" + isCallFromConfiguration + "_" + campaignId.ToString();

            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cacheName) && campaignId != 0)
            {
                if (isCallFromConfiguration == true)
                {
                    cache.Remove(campaignId.ToString());
                    return CreateFreshCache(lotusServer, nsfFile, BPA.Utility.EncryptDecrypt.Decrypt(userPas), campaignId.ToString(), cache);
                }
                else
                {
                    return GetDatabase((NotesSession)cache.Get(campaignId.ToString()), lotusServer, nsfFile, false);
                }
            }
            else if (campaignId == 0)
            {
                NotesSession oNotesSession = new NotesSession();
                oNotesSession.Initialize(userPas);
                oNotesSession.ConvertMime = true;
                return GetDatabase(oNotesSession, lotusServer, nsfFile, false);
            }
            else
            {
                return CreateFreshCache(lotusServer, nsfFile, BPA.Utility.EncryptDecrypt.Decrypt(userPas), campaignId.ToString(), cache);
            }


        }

        private static NotesDatabase CreateFreshCache(string lotusServer, string nsfFile, string _userPas, string CampaignID, ObjectCache cache)
        {
            NotesSession oNotesSession = new NotesSession();
            oNotesSession.Initialize(_userPas);
            oNotesSession.ConvertMime = true;


            // Store data in the cache
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(GetCacheinterval());
            cache.Add(CampaignID, oNotesSession, cacheItemPolicy);
            return GetDatabase(oNotesSession, lotusServer, nsfFile, false);
        }

        public static string GetSetting(string val)
        {

            try
            {
                return ConfigurationManager.AppSettings[val];
            }
            catch
            {
                return "";
            }

        }

        private static int GetCacheinterval()
        {
            int intervalTime = 30;
            if (GetSetting("CacheInterval") != null)
            { intervalTime = int.Parse(GetSetting("CacheInterval")); }
            return intervalTime;
        }

        private static NotesDatabase GetDatabase(NotesSession ns, string lotusServer, string nsfFile, bool createOnFail)
        {
            return ns.GetDatabase(lotusServer, nsfFile, createOnFail);
        }

    }
}
