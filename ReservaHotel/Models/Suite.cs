using Newtonsoft.Json;

namespace ReservaHotel.Models
{
    /// <summary>
    /// Classe responsável por representar uma suíte, possuindo as propriedades do tipo da suíte, a capacidade e 
    /// o custo da diária.
    /// </summary>
    public class Suite
    {
        public Suite() { }

        /// <summary>
        /// Construtor da classe Suite
        /// </summary>
        /// <param name="tipoSuite">Referência qual o tipo da suíte</param>
        /// <param name="capacidade">Indica quantos hóspedes podem dormir na suíte</param>
        /// <param name="valorDiaria">Indica o custo por dia da suíte</param>
        public Suite(string tipoSuite, int capacidade, decimal valorDiaria)
        {
            TipoSuite = tipoSuite;
            Capacidade = capacidade;
            ValorDiaria = valorDiaria;
        }

        public int Capacidade { get; set; }
        [JsonProperty("Tipo")]
        public string TipoSuite { get; set; }
        [JsonProperty("ValorDiario")]
        public decimal ValorDiaria { get; set; }
        public int Id { get; set; }
    }
}