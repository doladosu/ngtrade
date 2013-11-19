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
            return GetNewsFromBloonberg();
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

                            //var pixarRows = from d in pixarTable.Descendants() where d.Name == "li" select d;
                            result = pixarTable.InnerHtml.Replace("\n", "").Replace("href=", "target=\"_blank\" href=").Replace("h3", "h4").Replace("â€™", "'");
                            cache.Add(NEWS_DETAILS_CACHE_KEY + sUrl, result, policy);

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
                            result = pixarRows.Select(pixarRow => pixarRow.InnerHtml.Replace("\n", "").Replace("href=", "target=\"_blank\" href=").Replace("h3", "h4").Replace("â€™", "'")).ToList();
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
    }
}