using Newtonsoft.Json;
using System;

namespace PixMicro.Core
{
    /// <summary>
    /// An image, encoded as a base64 string.
    /// </summary>
    public class Base64Image
    {
        public string Base64String { get; }
        public byte[] Bytes { get; }

        public Base64Image(string base64String)
        {
            var processedString = base64String;
            // Remove URI scheme, if present
            if (base64String.Contains(","))
            {
                processedString = base64String.Substring(base64String.IndexOf(",") + 1);
            }
            this.Base64String = processedString;
            this.Bytes = Convert.FromBase64String(this.Base64String);
        }

        public Base64Image(byte[] bytes)
        {
            this.Bytes = bytes;
            this.Base64String = Convert.ToBase64String(this.Bytes);
        }
    }

    public class Base64ImageJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Base64Image);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load the JSON for the Result into a JObject
            return new Base64Image((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((Base64Image)value).Base64String);
        }
    }
}
