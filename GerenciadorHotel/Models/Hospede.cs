using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorHotel.Models
{
    /// <summary>
    /// Classe que representa um hóspede
    /// </summary>
    public class Hospede
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeCompleto { get; set; }
    }
}