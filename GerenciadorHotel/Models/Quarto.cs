using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GerenciadorHotel.Models
{
    /// <summary>
    /// Classe que representa um quarto (suíte).
    /// </summary>
    public class Quarto
    {
        public int Capacidade { get; set; }
        public string Tipo { get; set; }
        public decimal ValorDiario { get; set; }
        public int Id { get; set; }

        /// <summary>
        /// Método para receber valores sobre o tipo da suíte, capacidade e valor diário.
        /// </summary>
        /// <returns>Retorna o tipo, capacidade e valor diário recebidos</returns>
        public (string tipo, int capacidade, decimal valorDiaria) AdicionarInformacoes()
        {
            Console.WriteLine("Escreva o tipo do quarto");
            string tipo = Console.ReadLine();
            Console.WriteLine("Digite a capacidade do quarto");
            Int32.TryParse(Console.ReadLine(), out int capacidade);
            Console.WriteLine("Digite o preço da diária");
            Decimal.TryParse(Console.ReadLine(), out decimal custoDiaria);

            return (tipo, capacidade, custoDiaria);
        }

        /// <summary>
        /// Irá receber as informações de tipo de suíte, capacidade e valor diário e atribui para o objeto.
        /// </summary>
        public void AtribuirInformacoes()
        {
            (string tipo, int capacidade, decimal valorDiaria) = AdicionarInformacoes();

            Capacidade = capacidade;
            Tipo = tipo;
            ValorDiario = valorDiaria;
        }
    }
}