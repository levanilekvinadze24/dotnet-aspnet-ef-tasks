using System.Net;
using System.Text.Json;

namespace CountryServices
{
    public class CountryService : ICountryService, IDisposable
    {
        private const string ServiceUrl = "https://restcountries.com/v2";
        private readonly HttpClient httpClient;
        private readonly Dictionary<string, WeakReference<LocalCurrency>> currencyCountries = new();
        private bool disposed;

        public CountryService()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(ServiceUrl),
                Timeout = TimeSpan.FromSeconds(30),
            };
        }

        public LocalCurrency GetLocalCurrencyByAlpha2Or3Code(string? alpha2Or3Code)
        {
            if (string.IsNullOrWhiteSpace(alpha2Or3Code))
            {
                throw new ArgumentException("Country code cannot be null or whitespace.", nameof(alpha2Or3Code));
            }

            var lowerCode = alpha2Or3Code.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (this.currencyCountries.TryGetValue(lowerCode, out var weakReference) &&
                weakReference.TryGetTarget(out var cachedCurrency))
            {
                return cachedCurrency;
            }

            using var webClient = new WebClient();
            string url = $"{ServiceUrl}/alpha/{alpha2Or3Code}?fields=name,currencies";

            try
            {
                string json = webClient.DownloadString(url);
                var countryInfo = JsonSerializer.Deserialize<LocalCurrencyInfo>(json);

                if (countryInfo?.Currencies == null || countryInfo.Currencies.Length == 0 ||
                    string.IsNullOrEmpty(countryInfo.Currencies[0].Code))
                {
                    throw new ArgumentException("Invalid country code.", nameof(alpha2Or3Code));
                }

                var currency = countryInfo.Currencies[0];
                var localCurrency = new LocalCurrency
                {
                    CountryName = countryInfo.CountryName,
                    CurrencyCode = currency.Code,
                    CurrencySymbol = currency.Symbol,
                };

                this.currencyCountries[lowerCode] = new WeakReference<LocalCurrency>(localCurrency);
                return localCurrency;
            }
            catch (WebException ex)
            {
                throw new ArgumentException("Invalid country code.", nameof(alpha2Or3Code), ex);
            }
        }

        public async Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(string? alpha2Or3Code, CancellationToken token)
        {
            ValidateAlphaCode(alpha2Or3Code);

            var lowerCode = alpha2Or3Code!.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (this.currencyCountries.TryGetValue(lowerCode, out var weakReference) &&
                weakReference.TryGetTarget(out var cachedCurrency))
            {
                return cachedCurrency;
            }

            return await this.FetchCurrencyAsync(alpha2Or3Code, lowerCode, token).ConfigureAwait(false);
        }

        public Country GetCountryInfoByCapital(string? capital)
        {
            if (string.IsNullOrWhiteSpace(capital))
            {
                throw new ArgumentException("Capital name cannot be null or whitespace.", nameof(capital));
            }

            using var webClient = new WebClient();
            string encodedCapital = Uri.EscapeDataString(capital);
            string url = $"{ServiceUrl}/capital/{encodedCapital}?fields=name,capital,area,population,flag";

            try
            {
                string json = webClient.DownloadString(url);
                var countryInfos = JsonSerializer.Deserialize<CountryInfo[]>(json);

                if (countryInfos == null || countryInfos.Length == 0)
                {
                    throw new ArgumentException("No country found with this capital.", nameof(capital));
                }

                var countryInfo = countryInfos[0];
                return new Country
                {
                    Name = countryInfo.Name,
                    CapitalName = countryInfo.CapitalName,
                    Area = countryInfo.Area,
                    Population = countryInfo.Population,
                    Flag = countryInfo.Flag,
                };
            }
            catch (WebException ex)
            {
                throw new ArgumentException("No country found with this capital.", nameof(capital), ex);
            }
        }

        public async Task<Country> GetCountryInfoByCapitalAsync(string? capital, CancellationToken token)
        {
            ValidateCapital(capital);
            return await this.FetchCountryByCapitalAsync(capital!, token).ConfigureAwait(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.httpClient?.Dispose();
                }

                this.disposed = true;
            }
        }

        private static void ValidateAlphaCode(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Country code cannot be null or whitespace.", nameof(code));
            }
        }

        private static void ValidateCapital(string? capital)
        {
            if (string.IsNullOrWhiteSpace(capital))
            {
                throw new ArgumentException("Capital name cannot be null or whitespace.", nameof(capital));
            }
        }

        private async Task<LocalCurrency> FetchCurrencyAsync(string alphaCode, string lowerCode, CancellationToken token)
        {
            var uri = new Uri(this.httpClient.BaseAddress!, $"/alpha/{alphaCode}?fields=name,currencies");

            try
            {
                var response = await this.httpClient.GetAsync(uri, token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException("Invalid country code.", nameof(alphaCode));
                }

                await using var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
                var countryInfo = await JsonSerializer.DeserializeAsync<LocalCurrencyInfo>(stream, cancellationToken: token).ConfigureAwait(false);

                if (countryInfo?.Currencies == null || countryInfo.Currencies.Length == 0 ||
                    string.IsNullOrEmpty(countryInfo.Currencies[0].Code))
                {
                    throw new ArgumentException("Invalid country code.", nameof(alphaCode));
                }

                var currency = countryInfo.Currencies[0];
                var localCurrency = new LocalCurrency
                {
                    CountryName = countryInfo.CountryName,
                    CurrencyCode = currency.Code,
                    CurrencySymbol = currency.Symbol,
                };

                this.currencyCountries[lowerCode] = new WeakReference<LocalCurrency>(localCurrency);
                return localCurrency;
            }
            catch (HttpRequestException ex)
            {
                throw new ArgumentException("Invalid country code.", nameof(alphaCode), ex);
            }
        }

        private async Task<Country> FetchCountryByCapitalAsync(string capital, CancellationToken token)
        {
            var uri = new Uri(this.httpClient.BaseAddress!, $"/capital/{Uri.EscapeDataString(capital)}?fields=name,capital,area,population,flag");

            try
            {
                var response = await this.httpClient.GetAsync(uri, token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException("No country found with this capital.", nameof(capital));
                }

                await using var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
                var countryInfos = await JsonSerializer.DeserializeAsync<CountryInfo[]>(stream, cancellationToken: token).ConfigureAwait(false);

                if (countryInfos == null || countryInfos.Length == 0)
                {
                    throw new ArgumentException("No country found with this capital.", nameof(capital));
                }

                var countryInfo = countryInfos[0];
                return new Country
                {
                    Name = countryInfo.Name,
                    CapitalName = countryInfo.CapitalName,
                    Area = countryInfo.Area,
                    Population = countryInfo.Population,
                    Flag = countryInfo.Flag,
                };
            }
            catch (HttpRequestException ex)
            {
                throw new ArgumentException("No country found with this capital.", nameof(capital), ex);
            }
        }
    }
}
