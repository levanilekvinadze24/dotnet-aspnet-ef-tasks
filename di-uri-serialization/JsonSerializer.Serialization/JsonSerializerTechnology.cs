using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Serialization;
using UriSerializationHelper;

namespace JsonSerializer.Serialization
{
    public class JsonSerializerTechnology : IDataSerializer<Uri>
    {
        // Static members first (SA1204). Use explicit type to avoid SA1000/IDE0055 ping-pong.
        private static readonly JsonSerializerOptions CachedOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        private readonly string path;
        private readonly ILogger<JsonSerializerTechnology>? logger;

        public JsonSerializerTechnology(string? path, ILogger<JsonSerializerTechnology>? logger = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);
            this.path = path!;
            this.logger = logger;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var payload = source
                .Where(u => u is not null)
                .Select(u => u!.ToSerializableObject())
                .ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(payload, CachedOptions);
            File.WriteAllText(this.path, json);

            this.logger?.LogInformation("JSON written to {Path}. {Count} items.", this.path, payload.Count);
        }
    }
}
