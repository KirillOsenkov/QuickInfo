using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class ExchangeRates
    {
        private const string Endpoint = @"https://www.floatrates.com/daily/usd.json";
        private static readonly HttpClient httpClient = new HttpClient();

        private static DateTime currencyCacheCreated = DateTime.UtcNow;

        private static ExchangeRates instance;
        public static ExchangeRates Instance
        {
            get
            {
                var now = DateTime.UtcNow;
                if (instance == null ||
                    currencyCacheCreated == default ||
                    now - currencyCacheCreated > TimeSpan.FromDays(1))
                {
                    lock (httpClient)
                    {
                        currencyCacheCreated = now;
                        instance = new ExchangeRates();

                        try
                        {
                            string json = httpClient.GetStringAsync(Endpoint).Result;
                            instance.Rates = JsonConvert.DeserializeObject<Dictionary<string, ExchangeRate>>(json);
                        }
                        catch
                        {
                        }
                    }
                }

                return instance;
            }
        }

        public ExchangeRate Get(string currency)
        {
            if (Rates.TryGetValue(currency, out var rate))
            {
                return rate;
            }

            rate = Rates.Values.FirstOrDefault(r =>
                r.code.IndexOf(currency, StringComparison.OrdinalIgnoreCase) != -1 ||
                r.name.IndexOf(currency, StringComparison.OrdinalIgnoreCase) != -1);

            return rate;
        }

        public Dictionary<string, ExchangeRate> Rates { get; private set; } = new Dictionary<string, ExchangeRate>();
    }

    public class ExchangeRate
    {
        public string code { get; set; }
        public string alphaCode { get; set; }
        public string numericCode { get; set; }
        public string name { get; set; }
        public float rate { get; set; }
        public DateTime date { get; set; }
        public float inverseRate { get; set; }
    }

    public class Currency : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return new Node
                {
                    List = new List<object>
                    {
                        SectionHeader("Currency converter"),
                        HelpTable
                        (
                            ("150 EUR in USD", "")
                        )
                    }
                };
            }

            var list = query.OriginalInputTrim.SplitIntoWords();

            if (list != null && list.Count == 4)
            {
                if (!double.TryParse(list[0], out double firstNumber))
                {
                    return null;
                }

                var keyword = list[2];

                if (keyword == "in" || keyword == "to")
                {
                    string unit = list[1];
                    string tounit = list[3];
                    var result = Convert(unit, tounit, firstNumber);
                    if (result != 0.0)
                    {
                        return FixedParagraph($"{firstNumber} {unit} = {result:n2} {tounit}");
                    }
                }
            }

            return null;
        }

        public static double Convert(string from, string to, double value)
        {
            var rate = GetRate(from, to);
            return rate * value;
        }

        private static double GetRate(string from, string to)
        {
            var rateTo = GetRateToPrimary(from);
            if (rateTo == 0)
            {
                return 0;
            }

            var rateFrom = GetRateFromPrimary(to);
            if (rateFrom == 0)
            {
                return 0;
            }

            return rateTo * rateFrom;
        }

        private const string PrimaryRate = "USD";

        private static double GetRateToPrimary(string from)
        {
            if (string.Equals(from, PrimaryRate, StringComparison.OrdinalIgnoreCase))
            {
                return 1.0;
            }

            var rate = ExchangeRates.Instance.Get(from);
            if (rate == null)
            {
                return 1.0;
            }

            return rate.inverseRate;
        }

        private static double GetRateFromPrimary(string to)
        {
            if (string.Equals(to, PrimaryRate, StringComparison.OrdinalIgnoreCase))
            {
                return 1.0;
            }

            var rate = ExchangeRates.Instance.Get(to);
            if (rate == null)
            {
                return 1.0;
            }

            return rate.rate;
        }
    }
}
