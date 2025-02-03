using Newtonsoft.Json;


namespace testwork.Handlers
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private const string URL = "https://www.cbr-xml-daily.ru/daily_json.js";
        public CurrencyService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<Dictionary<string, Currency>> GetCurrenciesAsync()
        {
            var response = await _httpClient.GetStringAsync(URL);
            var data = JsonConvert.DeserializeObject<CurrencyResponse>(response);
            return data?.Valute ?? new();
        }

        public async Task<IEnumerable<Currency>> GetPagedCurrenciesAsync(int page, int pageSize)
        {
            var currencies = await GetCurrenciesAsync();
            return currencies.Values.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Currency?> GetCurrencyByIdAsync(string id)
        {
            var currencies = await GetCurrenciesAsync();
            foreach (var i in currencies.Keys)
            {
                if (currencies[i].Id == id)
                    return currencies[i];
                
            }
            
            return null;
        }

    }

    public class CurrencyResponse
    {
        [JsonProperty("Valute")]
        public Dictionary<string, Currency> Valute { get; set; } = new();
    }

    public class Currency
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("NumCode")]
        public string NumCode { get; set; }

        [JsonProperty("CharCode")]
        public string CharCode { get; set; }

        [JsonProperty("Nominal")]
        public int Nominal { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public decimal Value { get; set; }

        [JsonProperty("Previous")]
        public decimal Previous { get; set; }
    }
}
