using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace UriSerializationHelper
{
    /// <summary>
    /// Serializable DTO for one URI address (works for XML + JSON).
    /// </summary>
    public class UriAddress
    {
        // Inner elements for XmlSerializer; hidden from JSON.
        [JsonIgnore]
        [XmlElement("scheme")]
        public InnerNamed AttrScheme { get; set; }

        [JsonIgnore]
        [XmlElement("host")]
        public InnerNamed AttrHost { get; set; }

        // Expose lowercase names for JSON; ignored by XmlSerializer.
        [XmlIgnore]
        [JsonPropertyName("scheme")]
        public string Scheme
        {
            get => this.AttrScheme.Name;
            set => this.AttrScheme = new InnerNamed { Name = value };
        }

        [XmlIgnore]
        [JsonPropertyName("host")]
        public string Host
        {
            get => this.AttrHost.Name;
            set => this.AttrHost = new InnerNamed { Name = value };
        }

        [XmlArray("path")]
        [XmlArrayItem("segment")]
        [JsonPropertyName("path")]
        public List<string> Path { get; set; } = [];

        [JsonIgnore]
        [XmlIgnore]
        public List<QueryElement> Query { get; set; } = [];

        [JsonPropertyName("query")]
        [XmlArray("query")]
        [XmlArrayItem("parameter")]
        public List<QueryElement>? QuerySerializable => this.Query.Count > 0 ? this.Query : null;

        public struct InnerNamed
        {
            [XmlAttribute("name")]
            public string Name { get; set; }
        }
    }
}
