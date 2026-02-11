using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Serialization;

namespace XDomWriter.Serialization
{
    public class XDomTechnology : IDataSerializer<Uri>
    {
        // Cached logging delegate (fixes CA1848)
        private static readonly Action<ILogger, string, Exception?> LogWritten =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1, nameof(XDomTechnology)),
                "XML (X-DOM) written to {Path}.");

        private readonly string path;
        private readonly ILogger<XDomTechnology>? logger;

        public XDomTechnology(string? path, ILogger<XDomTechnology>? logger = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);
            this.path = path!;
            this.logger = logger;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var root = new XElement("uriAdresses"); // exact spelling

            foreach (var uri in source)
            {
                if (uri is null)
                {
                    continue;
                }

                var address = new XElement(
                    "uriAdress",
                    new XElement("scheme", new XAttribute("name", uri.Scheme)),
                    new XElement("host", new XAttribute("name", uri.Host)));

                var pathEl = new XElement("path");
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                foreach (var seg in segments)
                {
                    pathEl.Add(new XElement("segment", seg));
                }

                address.Add(pathEl);

                var query = uri.Query;
                if (!string.IsNullOrEmpty(query))
                {
                    // Avoid range operator to prevent IDE0055/SA1009 style nags
                    string raw = (query.Length > 0 && query[0] == '?')
                        ? query.Substring(1)
                        : query;

                    if (!string.IsNullOrWhiteSpace(raw))
                    {
                        var pairs = raw.Split('&', StringSplitOptions.RemoveEmptyEntries);
                        if (pairs.Length > 0)
                        {
                            var queryEl = new XElement("query");

                            foreach (var pair in pairs)
                            {
                                // Specify comparison explicitly (CA1307)
                                var idx = pair.IndexOf('=', StringComparison.Ordinal);
                                if (idx <= 0)
                                {
                                    continue;
                                }

                                var key = Uri.UnescapeDataString(pair.Substring(0, idx));

                                string val;
                                if (idx + 1 < pair.Length)
                                {
                                    val = Uri.UnescapeDataString(pair.Substring(idx + 1));
                                }
                                else
                                {
                                    val = string.Empty;
                                }

                                queryEl.Add(new XElement(
                                    "parameter",
                                    new XAttribute("key", key),
                                    new XAttribute("value", val)));
                            }

                            if (queryEl.HasElements)
                            {
                                address.Add(queryEl);
                            }
                        }
                    }
                }

                root.Add(address);
            }

            var doc = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            doc.Save(this.path);

            if (this.logger is not null)
            {
                LogWritten(this.logger, this.path, null);
            }
        }
    }
}
