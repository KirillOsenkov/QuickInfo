using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace QuickInfo
{
    public static class Currency
    {
        private const string Endpoint = @"https://api.exchangeratesapi.io/latest";

        private static readonly HttpClient _httpClient = new HttpClient();

        private static readonly Dictionary<string, double> currencyCache = new Dictionary<string, double>();
        private static System.DateTime currencyCacheCreated = System.DateTime.UtcNow;

        public static double Convert(string from, string to, double value)
        {
            var rate = GetRate(from, to);
            return rate * value;
        }

        private static double GetRate(string from, string to)
        {
            string cacheKey = from + to;
            double rateValue;

            lock (currencyCache)
            {
                if ((System.DateTime.UtcNow - currencyCacheCreated) > TimeSpan.FromDays(1))
                {
                    currencyCache.Clear();
                    currencyCacheCreated = System.DateTime.UtcNow;
                }

                if (currencyCache.TryGetValue(cacheKey, out rateValue))
                {
                    return rateValue;
                }
            }

            from = from.ToUpper();
            to = to.ToUpper();

            var endpoint = $"{Endpoint}?base={from}&symbols={to}";
            var result = _httpClient.GetStringAsync(endpoint).Result;
            var rate = JsonConvert.DeserializeObject<CurrencyRate>(result);
            var rates = rate.Rates;

            if (rates == null || rates.Count != 1)
            {
                return 0;
            }

            rateValue = rates[to];

            lock (currencyCache)
            {
                currencyCache[cacheKey] = rateValue;
            }

            return rateValue;
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
