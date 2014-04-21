using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Adform.Academy.DataTransfer.WebApi.Formatters
{
    public class JsonFormatter : MediaTypeFormatter
    {
        private static readonly JsonSerializer Serializer;

        static JsonFormatter()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

#if DEBUG
            settings.Formatting = Formatting.Indented;
#endif
            Serializer = JsonSerializer.Create(settings);
        }

        public JsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var tcs = new TaskCompletionSource<object>();

            try
            {
                using (var reader = new JsonTextReader(new StreamReader(readStream)) { CloseInput = false })
                {
                    tcs.SetResult(Serializer.Deserialize(reader, type));
                }
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var tcs = new TaskCompletionSource<object>();

            try
            {
                using (var writer = new JsonTextWriter(new StreamWriter(writeStream)) { CloseOutput = false })
                {
                    Serializer.Serialize(writer, value);
                    writer.Flush();
                }

                tcs.SetResult(null);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }
    }
}
