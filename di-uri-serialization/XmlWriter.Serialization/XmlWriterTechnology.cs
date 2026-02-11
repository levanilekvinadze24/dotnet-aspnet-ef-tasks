using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using Serialization;

namespace XmlWriter.Serialization
{
    public class XmlWriterTechnology : IDataSerializer<Uri>
    {
        // Cache logging delegate (fixes CA1848)
        private static readonly Action<ILogger, string, Exception?> LogWritten =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1, nameof(XmlWriterTechnology)),
                "XML written to {Path}.");

        private readonly string path;
        private readonly ILogger<XmlWriterTechnology>? logger;

        public XmlWriterTechnology(string? path, ILogger<XmlWriterTechnology>? logger = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);
            this.path = path!;
            this.logger = logger;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
            };

            using var writer = System.Xml.XmlWriter.Create(this.path, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("uriAdresses"); // exact spelling per tests

            foreach (var uri in source)
            {
                if (uri is null)
                {
                    continue;
                }

                writer.WriteStartElement("uriAdress");

                writer.WriteStartElement("scheme");
                writer.WriteAttributeString("name", uri.Scheme);
                writer.WriteEndElement();

                writer.WriteStartElement("host");
                writer.WriteAttributeString("name", uri.Host);
                writer.WriteEndElement();

                writer.WriteStartElement("path");
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                foreach (var seg in segments)
                {
                    writer.WriteElementString("segment", seg);
                }

                writer.WriteEndElement(); // path

                var query = uri.Query;
                if (!string.IsNullOrEmpty(query))
                {
                    // Avoid range operator to prevent IDE0055/SA1009 nitpicks
                    string raw = (query.Length > 0 && query[0] == '?')
                        ? query.Substring(1)
                        : query;

                    if (!string.IsNullOrWhiteSpace(raw))
                    {
                        var pairs = raw.Split('&', StringSplitOptions.RemoveEmptyEntries);
                        if (pairs.Length > 0)
                        {
                            writer.WriteStartElement("query");
                            foreach (var pair in pairs)
                            {
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

                                writer.WriteStartElement("parameter");
                                writer.WriteAttributeString("key", key);
                                writer.WriteAttributeString("value", val);
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement(); // query
                        }
                    }
                }

                writer.WriteEndElement(); // uriAdress
            }

            writer.WriteEndElement(); // uriAdresses
            writer.WriteEndDocument();

            if (this.logger is not null)
            {
                LogWritten(this.logger, this.path, null);
            }
        }
    }
}
