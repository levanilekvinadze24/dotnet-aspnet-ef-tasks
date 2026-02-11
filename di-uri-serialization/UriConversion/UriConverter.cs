using Conversion;
using LogerExtensionDelegate;
using Microsoft.Extensions.Logging;
using Validation;

namespace UriConversion
{
    /// <summary>
    /// The converter class from string to <see cref="Uri"/>.
    /// </summary>
    public sealed class UriConverter : IConverter<Uri?>
    {
        private readonly IValidator<string> validator;
        private readonly ILogger<UriConverter>? logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriConverter"/> class.
        /// </summary>
        /// <param name="validator">The string validator.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">Thrown if validator is null.</exception>
        public UriConverter(IValidator<string>? validator, ILogger<UriConverter>? logger = default)
        {
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.logger = logger;
        }

        /// <summary>
        /// Converts the source string to a <see cref="Uri"/> object.
        /// </summary>
        /// <param name="obj">The source string.</param>
        /// <returns>The <see cref="Uri"/> object if valid; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source string is null.</exception>
        public Uri? Convert(string? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (!this.validator.IsValid(obj))
            {
                LogerExtension.FastLoggerMessage(
                    this.logger,
                    $"Invalid URI skipped: '{obj}'",
                    new LogerExtensionException("Validation failed for source URI string."));
                return null;
            }

            try
            {
                return new Uri(obj, UriKind.Absolute);
            }
            catch (UriFormatException ex)
            {
                LogerExtension.FastLoggerMessage(
                    this.logger,
                    $"Failed to create Uri: '{obj}'",
                    new LogerExtensionException("Uri format is invalid.", ex));
                return null;
            }
        }
    }
}
