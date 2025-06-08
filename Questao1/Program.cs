using System;
using System.Globalization;

namespace Questao1 {
    class Program
    {
        static ContaBancaria conta;
        static void Main(string[] args)
        {
            Console.Write("Entre o número da conta: ");
            int numero;
            while (!int.TryParse(Console.ReadLine(), out numero))
            {
                Console.Write("Valor inválido. Por favor, digite um número inteiro: ");
            }

            Console.Write("Entre o titular da conta: ");
            string titular = Console.ReadLine();

            Console.Write("Haverá depósito inicial (s/n)? ");
            char resp;
            while (!char.TryParse(Console.ReadLine(), out resp) || (resp != 's' && resp != 'S' && resp != 'n' && resp != 'N'))
            {
                Console.Write("Valor inválido. Por favor, digite 's' ou 'n': ");
            }

            if (resp == 's' || resp == 'S')
            {
                Console.Write("Entre o valor de depósito inicial: ");
                double depositoInicial;
                while (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out depositoInicial))
                {
                    Console.Write("Valor inválido. Por favor, digite um valor numérico válido: ");
                }
                conta = new ContaBancaria(numero, titular, depositoInicial);
            }
            else
            {
                conta = new ContaBancaria(numero, titular);
            }

            ExibeDadosDaConta(conta);
            MovimentacaoBancaria("depósito");
            MovimentacaoBancaria("saque");

            Console.WriteLine();
            Console.Write("Gostaria de alterar o titular da conta (s/n)? ");
            while (!char.TryParse(Console.ReadLine(), out resp) || (resp != 's' && resp != 'S' && resp != 'n' && resp != 'N'))
            {
                Console.Write("Valor inválido. Por favor, digite 's' ou 'n': ");
            }
            if (resp == 's' || resp == 'S')
            {
                Console.Write("Entre o novo titular da conta: ");
                string novoTitular = Console.ReadLine();
                conta.NomeTitular = novoTitular;
                Console.WriteLine("Titular atualizado com sucesso!");
            }
            
            ExibeDadosDaConta(conta);
        }
        private static void MovimentacaoBancaria(string tipo)
        {
            Console.WriteLine();
            Console.WriteLine($"Entre um valor para {tipo}: ");
            double quantia;
            while (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out quantia))
            {
                Console.Write($"Valor inválido. Por favor, digite um valor numérico válido para {tipo}: ");
            }
            if (tipo == "saque")
            {
                try
                {
                    conta.Saque(quantia);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine($"Erro ao realizar saque: {e.Message}");
                }
            }
            else if (tipo == "depósito")
            {
                try
                {
                    conta.Deposito(quantia);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Erro ao realizar depósito: {e.Message}");
                }
            }
            ExibeDadosDaConta(conta);
        }

        private static void ExibeDadosDaConta(ContaBancaria conta)
        {
            Console.WriteLine();
            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine(conta.ToString());
        }
    }
}
/* Output expected:
        Exemplo 1:
            Entre o número da conta: 5447
            Entre o titular da conta: Milton Gonçalves
            Haverá depósito inicial(s / n) ? s
            Entre o valor de depósito inicial: 350.00

            Dados da conta:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

            Entre um valor para depósito: 200
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

            Entre um valor para saque: 199
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

        Exemplo 2:
            Entre o número da conta: 5139
            Entre o titular da conta: Elza Soares
            Haverá depósito inicial(s / n) ? n

            Dados da conta:
            Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

            Entre um valor para depósito: 300.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

            Entre um valor para saque: 298.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
        */