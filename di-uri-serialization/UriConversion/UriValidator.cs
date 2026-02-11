using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Validation;

namespace UriConversion
{
    /// <summary>
    /// Uri string validator.
    /// </summary>
    public class UriValidator : IValidator<string>
    {
        // Optional stricter check for query pairs key=value&key2=value2...
        // We don't *require* a query, but if present it must be "key=value" pairs.
        private static readonly Regex QueryPairRegex = new Regex(
            @"^([^=&?#]+=[^=&?#]*)(?:&[^=&?#]+=[^=&?#]*)*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Initializes a new instance of the <see cref="UriValidator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UriValidator(ILogger<UriValidator>? logger = default)
        {
            // logger intentionally unused, left for DI signature compatibility
            _ = logger;
        }

        /// <summary>
        /// Determines if a string is valid Uri.
        /// </summary>
        /// <param name="obj">The source string.</param>
        /// <returns>true if the uri string is valid; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Throw if source string is null.</exception>
        public bool IsValid(string? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            // Must be an absolute http/https URI with a non-empty host.
            if (!Uri.TryCreate(obj, UriKind.Absolute, out var uri))
            {
                return false;
            }

            if (!string.Equals(uri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(uri.Host))
            {
                return false;
            }

            // If a query exists, ensure it's key=value pairs (as per task spec).
            // Uri.Query starts with '?', so trim it before checking.
            if (!string.IsNullOrEmpty(uri.Query))
            {
                var q = uri.Query.Length > 0 && uri.Query[0] == '?'
                    ? uri.Query.Substring(1)
                    : uri.Query;

                // Empty query like "?" is invalid by the spec we enforce.
                if (string.IsNullOrWhiteSpace(q))
                {
                    return false;
                }

                // Accept only k=v pairs joined by &.
                if (!QueryPairRegex.IsMatch(q))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
