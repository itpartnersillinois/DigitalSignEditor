using System;
using System.Linq;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.Twitter {

    public class TwitterCache : ITwitterCache {
        private string prefix = "twitter-";
        private ISignRepository signRepository;

        public TwitterCache(ISignRepository signRepository) {
            this.signRepository = signRepository;
        }

        public bool AddCache(string username, string data) {
            var cacheItem = signRepository.Read(sr => sr.CacheItems.FirstOrDefault(c => c.Name == prefix + username));
            if (cacheItem != null) {
                cacheItem.Data = data;
                cacheItem.LastUpdated = DateTime.Now;
                _ = signRepository.Update(cacheItem);
                return false;
            } else {
                _ = signRepository.Create(new CacheItem {
                    Data = data,
                    LastUpdated = DateTime.Now,
                    MinutesUntilCacheExpires = 60,
                    IsActive = true,
                    Name = prefix + username
                });
                return true;
            }
        }

        public string GetCache(string username) {
            var cacheItem = signRepository.Read(sr => sr.CacheItems.FirstOrDefault(c => c.Name == prefix + username));
            return cacheItem != null && cacheItem.LastUpdated.AddMinutes(cacheItem.MinutesUntilCacheExpires) > DateTime.Now ? cacheItem.Data : string.Empty;
        }
    }
}