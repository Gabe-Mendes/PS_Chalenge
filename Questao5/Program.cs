using MediatR;
using Microsoft.OpenApi.Models;
using Questao5.Infrastructure.Database.Repository;
using Questao5.Infrastructure.Services.Extensions;
using Questao5.Infrastructure.Services.Middleware;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Swagger;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Questão 5", Version = "v1" });
    c.SchemaFilter<Questao5.Infrastructure.Swagger.EnumSchemaFilter>();
    
    // Adicionar documentação para o cabeçalho de idempotência
    c.AddSecurityDefinition("Idempotency", new OpenApiSecurityScheme
    {
        Description = "Chave de idempotência para garantir que operações não sejam duplicadas",
        Name = "X-Chave-Idempotencia",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
});

// Configure MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Configure SQLite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Register the repository
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Questao5 API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Adicionar middlewares de idempotência na ordem correta
// Primeiro verifica se já existe (IdempotenciaMiddleware)
app.UseMiddleware<IdempotenciaMiddleware>();
// Depois captura a resposta para salvar (ResponseCapturaMiddleware)
app.UseMiddleware<ResponseCapturaMiddleware>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbBootstrap = scope.ServiceProvider.GetRequiredService<IDatabaseBootstrap>();
    dbBootstrap.Setup();
}

app.Run();