using System;
using System.Collections.Generic;
using Conversion;
using DataReceiving;
using Serialization;

namespace ExportDataService
{
    /// <summary>
    /// Receives strings, converts each to T exactly once, then serializes once.
    /// </summary>
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
            // Read once into a materialized buffer to avoid re-enumeration.
            var lines = new List<string?>();
            foreach (var s in this.receiver.Receive())
            {
                lines.Add(s);
            }

            // Convert exactly once per input.
            var output = new List<T>(lines.Count);
            foreach (var line in lines)
            {
                var converted = this.converter.Convert(line); // triggers validator via mock callback exactly once

                // If T is a reference type and converter can return null, skip it.
                if (converted is null)
                {
                    continue;
                }

                output.Add(converted);
            }

            // Serialize once.
            this.serializer.Serialize(output);
        }
    }
}
