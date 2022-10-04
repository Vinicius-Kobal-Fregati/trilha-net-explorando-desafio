using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservaHotel.Models
{
    /// <summary>
    /// Classe que será utilizada para gravas dados no arquivo de reservas
    /// </summary>
    public class DadosReserva
    {
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public List<Pessoa> Hospedes { get; set; }
        public decimal ValorTotal { get; set; }
        public int Id { get; set; }

        public DadosReserva() {}

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="entrada">Deve ser passado a data de entrada, no formato dia/mês/ano, ela deve ser maior que o dia atual</param>
        /// <param name="saida">Deve ser passado a data de saída, no formato dia/mês/ano, ela deve ser superior a da entrada</param>
        /// <param name="nomesCompletos">Lista de pessoas onde será usado o nome completo delas</param>
        /// <param name="valorTotal">Custo total da reserva</param>
        /// <param name="id">Id referente ao quarto de reserva</param>
        public DadosReserva(DateTime entrada, DateTime saida, List<Pessoa> nomesCompletos, decimal valorTotal, int id)
        {
            Entrada = entrada;
            Saida = saida;
            Hospedes = nomesCompletos;
            ValorTotal = valorTotal;
            Id = id;
        }
    }
}