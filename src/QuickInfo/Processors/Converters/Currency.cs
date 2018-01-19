using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace QuickInfo
{        
    public static class Currency
    {
        private const string Endpoint = @"https://api.fixer.io/latest";

        private static readonly MemoryCacheOptions _options = new MemoryCacheOptions();
        private static readonly MemoryCache _cache = new MemoryCache(_options);
        private static readonly HttpClient _httpClient = new HttpClient();

        public static double Convert(string from, string to, double value)
        {
            var pair = new CurrencyPair(from, to);
            var rate = _cache.GetOrCreate(
                pair,
                e => 
                {
                    e.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return GetRate(from, to);
                });

            return rate * value;
        }

        private static double GetRate(string from, string to)
        {
            var endpoint = $"{Endpoint}?base={from.ToUpper()}&symbols={to.ToUpper()}";
            var result = _httpClient.GetStringAsync(endpoint).Result;
            var rate = JsonConvert.DeserializeObject<CurrencyRate>(result);

            return rate.Rates.FirstOrDefault().Value;
        }
    }

    #region Exchange currencies stuff
    struct CurrencyPair
    {
        public CurrencyPair(string from, string to)
        {
            From = from;
            To = to;
        }

        public string From { get; set; }
        public string To { get; set; }
    }

    class CurrencyRate
    {
        [JsonProperty(PropertyName = "rates")]
        public Dictionary<string, double> Rates { get; set; }
    }
    #endregion
}
