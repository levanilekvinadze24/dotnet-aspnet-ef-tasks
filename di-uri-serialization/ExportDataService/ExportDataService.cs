using Conversion;
using DataReceiving;
using Serialization;

namespace ExportDataService
{
    public sealed class ExportDataService<T>
    {
        private readonly IDataReceiver receiver;
        private readonly IDataSerializer<T> serializer;
        private readonly IConverter<T> converter;

        public ExportDataService(
            IDataReceiver receiver,
            IDataSerializer<T> serializer,
            IConverter<T> converter)
        {
            this.receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public void Run()
        {
            var lines = this.receiver.Receive().ToList();

            var buffer = new List<T>(lines.Count);
            foreach (var line in lines)
            {
                var item = this.converter.Convert(line);
                if (item is not null)
                {
                    buffer.Add(item);
                }
            }

            this.serializer.Serialize(buffer);
        }
    }
}
