using System.Xml.Serialization;

namespace UriSerializationHelper
{
    /// <summary>
    /// Root container for XmlSerializer with EXACT names expected by tests.
    /// </summary>
    [XmlRoot(ElementName = "uriAdresses")]
    public class UriContainer
    {
        public UriContainer()
        {
            this.UriAddresses = [];
        }

        public UriContainer(IEnumerable<UriAddress> source)
        {
            this.UriAddresses = source?.ToArray() ?? [];
        }

        [XmlElement("uriAdress")]
        public UriAddress[] UriAddresses { get; set; }
    }
}
