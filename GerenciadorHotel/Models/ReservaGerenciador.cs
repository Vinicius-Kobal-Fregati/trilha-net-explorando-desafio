using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GerenciadorHotel.Models
{
    /// <summary>
    /// Classe que representa uma reserva, contendo a data de entrada, saída, a lista dos hóspedes, seu valor total e 
    /// o id referente ao quarto.
    /// </summary>
    public class ReservaGerenciador
    {
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public List<Hospede> Hospedes { get; set; }
        [JsonProperty("ValorTotal")]
        public decimal Valor { get; set; }
        public int Id { get; set; }

        public ReservaGerenciador()
        {
            Hospedes = new List<Hospede>();
        }
    }
}