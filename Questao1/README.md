# Sistema de Conta Bancária

Implementação simples de conta bancária em C#

## Descrição

- Criação de uma conta bancária com ou sem depósito inicial
- Realização de operações de depósito e saque
- Atualização do nome do titular da conta
- Após cada operação, os dados atualizados da conta são visualizados

## Funcionalidades

- **Criação de conta**: Permite criar uma conta com número, titular e saldo inicial opcional
- **Depósito**: Adiciona valores à conta (valores negativos são rejeitados)
- **Saque**: Retira valores da conta com uma taxa fixa de R$ 3,50 por operação
- **Atualização de titular**: Permite alterar o nome do titular da conta
- **Exibição de dados**: Mostra os dados atualizados da conta após cada operação

## Estrutura do Projeto

- `Program.cs`: Contém a interface de linha de comando e a lógica principal do programa
- `ContaBancaria.cs`: Implementa a classe ContaBancaria com suas propriedades e métodos

## Regras de Negócio

- Saques têm uma taxa fixa de R$ 3,50
- Não é permitido realizar depósitos com valores negativos
- Não é permitido realizar saques com valores negativos
- É possível realizar saques que deixem o saldo negativo

### Exemplo 1: Conta com depósito inicial
```
Entre o número da conta: 5447
Entre o titular da conta: Milton Gonçalves
Haverá depósito inicial(s/n)? s
Entre o valor de depósito inicial: 350.00

Dados da conta:
Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

Entre um valor para depósito: 200
Dados da conta atualizados:
Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

Entre um valor para saque: 199
Dados da conta atualizados:
Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50
```

### Exemplo 2: Conta sem depósito inicial
```
Entre o número da conta: 5139
Entre o titular da conta: Elza Soares
Haverá depósito inicial(s/n)? n

Dados da conta:
Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

Entre um valor para depósito: 300.00
Dados da conta atualizados:
Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

Entre um valor para saque: 298.00
Dados da conta atualizados:
Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
```

## Tecnologias Utilizadas

- C# 10.0
- .NET 6.0
- Console Application
