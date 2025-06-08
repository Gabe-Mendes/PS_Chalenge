using Questao5.Infrastructure.Converters;
using System.Text.Json.Serialization;

namespace Questao5.Domain.Enumerators
{
    [JsonConverter(typeof(TipoMovimentoJsonConverter))]
    public enum TipoMovimento
    {
        Credito = 'C',
        Debito = 'D'
    }
}