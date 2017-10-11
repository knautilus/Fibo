using System.IO;

using Newtonsoft.Json;
using RestSharp.Serializers;

using Fibo.Utils;

namespace Fibo.Transport.Rest
{
    public class BigIntegerJsonSerializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        public BigIntegerJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            _serializer = serializer;
            _serializer.Converters.Add(new BigIntegerConverter());
        }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    _serializer.Serialize(jsonTextWriter, obj);
                    return stringWriter.ToString();
                }
            }
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public string ContentType
        {
            get { return "application/json"; }
            set { }
        }

        public static BigIntegerJsonSerializer Default => new BigIntegerJsonSerializer(Newtonsoft.Json.JsonSerializer.CreateDefault());
    }
}
