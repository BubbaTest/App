﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Nicarao.DAL.Model;
//
//    var solPrestamo = SolPrestamo.FromJson(jsonString);

namespace Nicarao.DAL.Model
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class SolPrestamo
    {
        [JsonProperty("exp")]
        public List<Exp> Exp { get; set; }
    }

    public partial class Exp
    {
        [JsonProperty("JsonSolicitud")]
        public List<JsonSolicitud> JsonSolicitud { get; set; }

        [JsonProperty("JsonMovimiento")]
        public List<JsonMovimiento> JsonMovimiento { get; set; }
    }

    public partial class JsonMovimiento
    {
        [JsonProperty("ExpedienteId")]
        public string ExpedienteId { get; set; }

        [JsonProperty("ArchivoId")]
        public string ArchivoId { get; set; }

        [JsonProperty("EntidadId")]
        public string EntidadId { get; set; }

        [JsonProperty("UbicacionId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public string UbicacionId { get; set; }

        [JsonProperty("UsuarioRegistraId")]
        public string UsuarioRegistraId { get; set; }

        [JsonProperty("EntidadRegistraId")]
        public string EntidadRegistraId { get; set; }

        [JsonProperty("EstadoId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public string EstadoId { get; set; }
    }

    public partial class JsonSolicitud
    {
        [JsonProperty("PrioridadId")]
        public string PrioridadId { get; set; }

        [JsonProperty("Observacion")]
        public string Observacion { get; set; }

        [JsonProperty("UsuarioRegistraId")]
        public string UsuarioRegistraId { get; set; }

        [JsonProperty("EntidadRegistraId")]
        public string EntidadRegistraId { get; set; }
    }

    public partial class SolPrestamo
    {
        public static List<SolPrestamo> FromJson(string json) => JsonConvert.DeserializeObject<List<SolPrestamo>>(json, Nicarao.DAL.Model.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<SolPrestamo> self) => JsonConvert.SerializeObject(self, Nicarao.DAL.Model.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
