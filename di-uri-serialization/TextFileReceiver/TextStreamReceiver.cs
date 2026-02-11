using DataReceiving;
using Microsoft.Extensions.Logging;

namespace TextFileReceiver
{
    /// <summary>
    /// The data receiver from a text file.
    /// </summary>
    public class TextStreamReceiver : IDataReceiver
    {
        // Cached logging delegate (fixes CA1848 and SA1000)
        private static readonly Action<ILogger, string, Exception?> LogFileNotFound =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1, nameof(TextStreamReceiver)),
                "Input file not found: {Path}");

        private readonly string path;
        private readonly ILogger<TextStreamReceiver>? logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextStreamReceiver"/> class.
        /// </summary>
        /// <param name="path">The path to the text file.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="path"/> is null or empty.</exception>
        public TextStreamReceiver(string? path, ILogger<TextStreamReceiver>? logger = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);
            this.path = path;
            this.logger = logger;
        }

        /// <summary>
        /// Receives lines from the text file.
        /// </summary>
        /// <returns>Non-empty lines, trimmed.</returns>
        public IEnumerable<string> Receive()
        {
            if (!File.Exists(this.path))
            {
                if (this.logger is not null)
                {
                    LogFileNotFound(this.logger, this.path, null);
                }

                return Array.Empty<string>();
            }

            // Stream the file lazily, trim each line, and skip blanks.
            return File.ReadLines(this.path)
                       .Where(static line => !string.IsNullOrWhiteSpace(line))
                       .Select(static line => line.Trim());
        }
    }
}
