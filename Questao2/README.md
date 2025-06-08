# Questão 2 - Consulta de Gols por Time

## Descrição do Projeto
Console em C# que consulta e exibe o número total de gols marcados por times de futebol em um determinado ano. A aplicação utiliza uma API externa (jsonmock.hackerrank.com/api/football_matches) para obter dados de partidas de futebol.

## Funcionalidades
- Consulta de gols marcados por um time específico em um ano determinado
- Exibição do total de gols marcados pelo time atuando como mandante ou como visitante
- Paginação de resultados da API

## Tecnologias Utilizadas
- C# (.NET 6.0)
- HttpClient para requisições HTTP

## Estrutura do Projeto
- **Program.cs**: Ponto de entrada da aplicação, inicializa instâncias de DataTeam e exibe os resultados
- **DataTeam.cs**: Classe principal que gerencia a consulta de dados e cálculo de gols
- **Models/ResultMatchesRequest.cs**: Modelo para requisições à API
- **Models/ResultMatchesResponse.cs**: Modelo para respostas da API

## Exemplo de Saída
```
Team Paris Saint-Germain scored 109 goals in 2013.

Team Chelsea scored 92 goals in 2014.
```

## Detalhes de Implementação
A aplicação faz requisições à API `https://jsonmock.hackerrank.com/api/football_matches` para obter dados sobre partidas de futebol. Para cada time, são feitas duas consultas:
1. Gols marcados como time da casa (team1)
2. Gols marcados como time visitante (team2)

A classe `DataTeam` gerencia essas consultas e soma os resultados para obter o total de gols marcados pelo time no ano especificado.

## Requisitos
- .NET 6.0 SDK
- Conexão com a internet para acessar a API externa