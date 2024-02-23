using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Gaia.Helpers
{
    public class KeysJsonConverter : JsonConverter
    {

        //private readonly Type[] _types;

        //public KeysJsonConverter(params Type[] types)
        //{
        //    _types = types;
        //}

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //JToken t = JToken.FromObject(value);

            //if (t.Type != JTokenType.Object)
            //{
            //  t.WriteTo(writer);
            //}
            //else
            //{
            //  JObject o = (JObject)t;
            //  IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

            //  o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

            //  o.WriteTo(writer);
            //}
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (objectType == typeof(decimal) || objectType == typeof(decimal?))
            {
                return Convert.ToDecimal(token.Value<string>());
            }

            if (objectType == typeof(bool) || objectType == typeof(bool?))
            {
                bool _ret = false;

                if (string.Equals("1", token.Value<string>(), StringComparison.OrdinalIgnoreCase))
                { _ret = true; }

                return _ret;
            }

            //if (token.Type == JTokenType.Boolean)
            //{
            //    if (string.Equals("1", str, StringComparison.OrdinalIgnoreCase))
            //    {
            //        str = "tt";
            //    }
            //    else if (string.Equals("0", str, StringComparison.OrdinalIgnoreCase))
            //    {
            //        str = "dd";
            //    }
            //}

            return token.Value<string>();
        }

        //public override bool CanRead
        //{
        //    get { return false; }
        //}

        public override bool CanWrite { get { return false; } }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.FullName == "System.Decimal")
                return true;

            if (objectType == typeof(decimal) || objectType == typeof(decimal?))
                return true;

            if (objectType.FullName == "System.Boolean")
                return true;

            if (objectType == typeof(Boolean) || objectType == typeof(Boolean?))
                return true;

            return false;
        }
    }
}
