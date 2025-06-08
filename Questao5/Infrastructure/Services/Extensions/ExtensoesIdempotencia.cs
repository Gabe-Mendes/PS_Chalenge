
using Questao5.Infrastructure.Services.Middleware;

namespace Questao5.Infrastructure.Services.Extensions
{
    public static class ExtensoesIdempotencia
    {
        public static IApplicationBuilder UseIdempotency(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IdempotenciaMiddleware>();
        }
    }
}