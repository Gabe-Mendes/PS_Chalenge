using Questao5.Domain.Enumerators;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Questao5.Infrastructure.Converters
{
    public class TipoMovimentoJsonConverter : JsonConverter<TipoMovimento>
    {
        public override TipoMovimento Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();
            
            if (string.IsNullOrEmpty(value))
                throw new JsonException("Valor inválido para TipoMovimento");
                
            // Aceita tanto "C"/"D" quanto "Credito"/"Debito"
            return value.ToUpper() switch
            {
                "C" => TipoMovimento.Credito,
                "D" => TipoMovimento.Debito,
                "CREDITO" => TipoMovimento.Credito,
                "DEBITO" => TipoMovimento.Debito,
                _ => throw new JsonException($"Valor inválido para TipoMovimento: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, TipoMovimento value, JsonSerializerOptions options)
        {
            // Serializa como "C" ou "D"
            writer.WriteStringValue(value == TipoMovimento.Credito ? "C" : "D");
        }
    }
}