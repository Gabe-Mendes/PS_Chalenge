using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("criar")]
        public async Task<IActionResult> CriarMovimento([FromBody] CriarMovimentoRequest request, [FromHeader(Name = "X-Chave-Idempotencia")] string? chaveIdempotencia = null)
        {
            //Valida o valor da chave de idempotência
            if (string.IsNullOrEmpty(chaveIdempotencia))
                return BadRequest("A Chave de idempotência é obrigatória.");

            // Atribuir a chave da idempotência à requisição para uso pelo middleware
            request.ChaveIdempotencia = chaveIdempotencia;
            var response = await _mediator.Send(request);

            if (!response.Sucesso)
                return BadRequest(new { erro = response.MensagemErro });

            return Ok(new { id = response.IdMovimento });
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> ObterSaldoContaCorrente(string idContaCorrente)
        {
            var request = new ObterSaldoContaCorrenteRequest { IdContaCorrente = idContaCorrente };
            var response = await _mediator.Send(request);

            if (!response.Sucesso)
                return BadRequest(new { erro = response.MensagemErro });

            return Ok(new
            {
                numeroConta = response.NumeroConta,
                nomeTitular = response.NomeTitular,
                data = response.DataConsulta,
                saldo = response.Saldo
            });
        }
    }
}