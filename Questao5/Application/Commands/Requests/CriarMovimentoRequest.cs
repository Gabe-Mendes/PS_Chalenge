using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentoRequest:IRequest<CriarMovimentoResponse>
    {
        public string? IdContaCorrente { get; set; }
        public TipoMovimento TipoMovimento { get; set; } // 'C' para crédito, 'D' para débito
        public decimal Valor { get; set; }        
        // Propriedade que não faz parte do modelo de requisição, valor preenchido pelo middleware de idempotência
        internal string? ChaveIdempotencia { get; set; }
    }
}