using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        public int NumeroConta { get; private set; }
        public string NomeTitular { get; set; }
        public double Saldo { get; set; }

        public ContaBancaria(int numeroConta, string nomeTitular, double saldoInicial = 0.0) {
            NumeroConta = numeroConta;
            NomeTitular = nomeTitular;
            Saldo = saldoInicial;
        }

        public void Deposito(double valor) {
            if (valor < 0) {
                throw new ArgumentException("O valor do depósito deve ser positivo.");
            }
            Saldo += valor;
        }

        public void Saque(double valor) {
            if (valor < 0) {
                throw new ArgumentException("O valor do saque deve ser positivo.");
            }
            if (valor > Saldo) {
                throw new InvalidOperationException("Saldo insuficiente para realizar o saque.");
            }
            Saldo -= valor + 3.50; // Taxa de saque
        }

        public override string ToString() {
            return $"Conta: {NumeroConta}, Titular: {NomeTitular}, Saldo: ${Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}
