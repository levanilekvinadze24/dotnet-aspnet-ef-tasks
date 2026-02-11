using System.Xml;
using Microsoft.Extensions.Logging;
using Serialization;

namespace XmlDomWriter.Serialization
{
    public class XmlDomTechnology : IDataSerializer<Uri>
    {
        // Cached logging delegate (fixes CA1848)
        private static readonly Action<ILogger, string, Exception?> LogWritten =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1, nameof(XmlDomTechnology)),
                "XML (XmlDocument) written to {Path}.");

        private readonly string path;
        private readonly ILogger<XmlDomTechnology>? logger;

        public XmlDomTechnology(string? path, ILogger<XmlDomTechnology>? logger = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);
            this.path = path!;
            this.logger = logger;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var doc = new XmlDocument();
            var decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            _ = doc.AppendChild(decl);

            var root = doc.CreateElement("uriAdresses"); // exact spelling
            _ = doc.AppendChild(root);

            foreach (var uri in source)
            {
                if (uri is null)
                {
                    continue;
                }

                var addressEl = doc.CreateElement("uriAdress");

                // scheme
                var schemeEl = doc.CreateElement("scheme");
                var schemeAttr = doc.CreateAttribute("name");
                schemeAttr.Value = uri.Scheme;
                _ = schemeEl.Attributes.Append(schemeAttr);
                _ = addressEl.AppendChild(schemeEl);

                // host
                var hostEl = doc.CreateElement("host");
                var hostAttr = doc.CreateAttribute("name");
                hostAttr.Value = uri.Host;
                _ = hostEl.Attributes.Append(hostAttr);
                _ = addressEl.AppendChild(hostEl);

                // path segments
                var pathEl = doc.CreateElement("path");
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                foreach (var seg in segments)
                {
                    var segEl = doc.CreateElement("segment");
                    segEl.InnerText = seg;
                    _ = pathEl.AppendChild(segEl);
                }

                _ = addressEl.AppendChild(pathEl);

                // query
                var query = uri.Query;
                if (!string.IsNullOrEmpty(query))
                {
                    var raw = query[0] == '?' ? query.Substring(1) : query; // avoids range operator
                    if (!string.IsNullOrWhiteSpace(raw))
                    {
                        var pairs = raw.Split('&', StringSplitOptions.RemoveEmptyEntries);
                        if (pairs.Length > 0)
                        {
                            var queryEl = doc.CreateElement("query");

                            foreach (var pair in pairs)
                            {
                                var idx = pair.IndexOf('=', StringComparison.Ordinal);
                                if (idx <= 0)
                                {
                                    continue;
                                }

                                var key = Uri.UnescapeDataString(pair.Substring(0, idx));

                                // No ternary, no ranges → avoids IDE0055 formatting suggestion
                                string val;
                                if (idx + 1 < pair.Length)
                                {
                                    val = Uri.UnescapeDataString(pair.Substring(idx + 1));
                                }
                                else
                                {
                                    val = string.Empty;
                                }

                                var pEl = doc.CreateElement("parameter");

                                var kAttr = doc.CreateAttribute("key");
                                kAttr.Value = key;
                                _ = pEl.Attributes.Append(kAttr);

                                var vAttr = doc.CreateAttribute("value");
                                vAttr.Value = val;
                                _ = pEl.Attributes.Append(vAttr);

                                _ = queryEl.AppendChild(pEl);
                            }

                            if (queryEl.HasChildNodes)
                            {
                                _ = addressEl.AppendChild(queryEl);
                            }
                        }
                    }
                }

                _ = root.AppendChild(addressEl);
            }

            using var fs = new FileStream(this.path, FileMode.Create, FileAccess.Write, FileShare.None);
            doc.Save(fs);

            if (this.logger is not null)
            {
                LogWritten(this.logger, this.path, null);
            }
        }
    }
}
