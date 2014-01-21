using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using HtmlAgilityPack;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class NewsRepository : INewsRepository
    {
        private static readonly object CacheLockObjectCurrentSales = new object();
        private const string ALL_NEWS_CACHE_KEY = "AllNewsCache";
        private const string NEWS_DETAILS_CACHE_KEY = "NewsDetailsCache";

        public List<string> NewsList()
        {
            return GetNewsFromNse();
        }

        public string NewsDetail(string sUrl)
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[NEWS_DETAILS_CACHE_KEY + sUrl] as string;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[NEWS_DETAILS_CACHE_KEY + sUrl] as string;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(12) };

                        using (var client = new WebClient())
                        {
                            // fetching HTML
                            string pixarHtml = client.DownloadString(sUrl);

                            var document = new HtmlDocument();
                            document.LoadHtml(pixarHtml);

                            var pixarTable = (from d in document.DocumentNode.Descendants()
                                              where d.Name == "div" && d.Id == "story_display"
                                              select d).First();

                            result = pixarTable.InnerHtml.Replace("\n", "").Replace("href=", "class=\"redirectLink\" href=").Replace("h3", "h4").Replace("â€™", "'");
                            cache.Add(NEWS_DETAILS_CACHE_KEY + sUrl, result, policy);

                            return result;
                        }
                    }
                }
                return result;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        private static List<string> GetNewsFromBloonberg()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[ALL_NEWS_CACHE_KEY] as List<string>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[ALL_NEWS_CACHE_KEY] as List<string>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(12) };

                        using (var client = new WebClient())
                        {
                            // fetching HTML
                            string pixarHtml = client.DownloadString("http://topics.bloomberg.com/nigerian-stock-exchange/");

                            var document = new HtmlDocument();
                            document.LoadHtml(pixarHtml);

                            var pixarTable = (from d in document.DocumentNode.Descendants()
                                                   where d.Name == "div" && d.Id == "stories"
                                                   select d).First();

                            var pixarRows = from d in pixarTable.Descendants() where d.Name == "li" select d;
                            result = pixarRows.Select(pixarRow => pixarRow.InnerHtml.Replace("\n", "").Replace("href=", "class=\"redirectLink\" href=").Replace("h3", "h4").Replace("â€™", "'")).ToList();
                            cache.Add(ALL_NEWS_CACHE_KEY, result, policy);

                            return result;
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string NewsDetailNse(string sUrl)
        {
            try
            {
                sUrl = "http://www.nse.com.ng" + sUrl;
                var cache = MemoryCache.Default;
                var result = cache[NEWS_DETAILS_CACHE_KEY + sUrl] as string;
                var resultBackup = cache[NEWS_DETAILS_CACHE_KEY + sUrl + "backup"] as string;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[NEWS_DETAILS_CACHE_KEY + sUrl] as string;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(12) };
                        var policyBackup = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(12) };

                        using (var client = new WebClient())
                        {
                            // fetching HTML
                            string pixarHtml = client.DownloadString(sUrl);

                            var document = new HtmlDocument();
                            document.LoadHtml(pixarHtml);

                            var pixarTable = (from d in document.DocumentNode.Descendants()
                                              where d.Name == "table" && d.Id == "Table_01"
                                              select d).First();

                            result = pixarTable.InnerHtml.Replace("src=\"/", "src=\"http://www.nse.com.ng/").Replace("href=", "class=\"redirectLink\" href=");
                            cache.Add(NEWS_DETAILS_CACHE_KEY + sUrl, result, policy);
                            if (!string.IsNullOrEmpty(result))
                            {
                                cache.Add(NEWS_DETAILS_CACHE_KEY + sUrl + "backup", result, policyBackup);
                            }
                            else if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(resultBackup))
                            {
                                result = resultBackup;
                            }
                            return result;
                        }
                    }
                }
                return result;
            }
            catch (Exception exception)
            {
                var cache = MemoryCache.Default;
                var resultBackup = cache[NEWS_DETAILS_CACHE_KEY + sUrl + "backup"] as string;
                return !string.IsNullOrEmpty(resultBackup) ? resultBackup : null;
            }
        }

        private static List<string> GetNewsFromNse()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[ALL_NEWS_CACHE_KEY] as List<string>;
                var resultBackup = cache[ALL_NEWS_CACHE_KEY + "backup"] as List<string>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[ALL_NEWS_CACHE_KEY] as List<string>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(12) };
                        var policyBackup = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(12) };

                        using (var client = new WebClient())
                        {
                            // fetching HTML
                            string pixarHtml = client.DownloadString("http://www.nse.com.ng/MarketNews/Pages/Recent-Happenings.aspx");

                            var document = new HtmlDocument();
                            document.LoadHtml(pixarHtml);

                            var pixarTable = (from d in document.DocumentNode.Descendants()
                                              where d.Name == "div" && d.Id == "WebPartWPQ4"
                                              select d).First();

                            if (result == null)
                            {
                                result = new List<string>();
                            }
                            result.Add(pixarTable.InnerHtml.Replace("href=", "class=\"redirectLink\" href=").Replace("â€œ", "\"").Replace("â€", "\"").Replace("â€‹", ""));
                            cache.Add(ALL_NEWS_CACHE_KEY, result, policy);
                            if (result.Count > 0)
                            {
                                cache.Add(ALL_NEWS_CACHE_KEY + "backup", result, policyBackup);
                            }
                            else if (result.Count == 0 && resultBackup != null && resultBackup.Count > 0)
                            {
                                result = resultBackup;
                            }
                            return result;
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                var cache = MemoryCache.Default;
                var resultBackup = cache[ALL_NEWS_CACHE_KEY + "backup"] as List<string>;
                if (resultBackup != null && resultBackup.Count > 0)
                {
                    return resultBackup;
                }
                return new List<string>();
            }
        }

    }
}