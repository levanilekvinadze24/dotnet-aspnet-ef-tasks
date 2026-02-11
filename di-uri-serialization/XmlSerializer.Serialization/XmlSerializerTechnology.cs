using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Serialization;
using UriSerializationHelper;

namespace XmlSerializer.Serialization
{
    /// <summary>
    /// Serializes IEnumerable&lt;Uri&gt; using XmlSerializer.
    /// </summary>
    public class XmlSerializerTechnology : IDataSerializer<Uri>
    {
        // ✅ Cached delegate to avoid CA1848
        private static readonly Action<ILogger, string, int, Exception?> LogWritten =
            LoggerMessage.Define<string, int>(
                LogLevel.Information,
                new EventId(1, nameof(XmlSerializerTechnology)),
                "XML (XmlSerializer) written to {Path}. {Count} items.");

        private readonly string path;
        private readonly ILogger<XmlSerializerTechnology>? logger;

        public XmlSerializerTechnology(string? path, ILogger<XmlSerializerTechnology>? logger = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);
            this.path = path!;
            this.logger = logger;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var list = source
                .Where(u => u is not null)
                .Select(u => u!.ToSerializableObject())
                .ToList();

            var container = new UriContainer(list);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                OmitXmlDeclaration = false,
            };

            // Suppress default namespaces (xsd/xsi) to match the expected output.
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(UriContainer));

            using var writer = XmlWriter.Create(this.path, settings);
            serializer.Serialize(writer, container, ns);

            // ✅ Use cached delegate instead of LogInformation
            if (this.logger is not null)
            {
                LogWritten(this.logger, this.path, list.Count, null);
            }
        }
    }
}
