using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GerenciadorHotel.Models
{

    /// <summary>
    /// Classe que interliga salva os dados nos arquivos reservas e quartosRegistrados, além de possuir as principais funcionalidades
    /// do sistema, como adicionar suíte, remover, cancelar reserva, etc...
    /// </summary>
    public class Gerenciador
    {

        /// <summary>
        /// Construtor da classe, ele verifica se os arquivos com os dados existem, caso não, inicia uma lista vazia das informações.
        /// Também realiza a limpeza de reservas que já foram finalizadas.
        /// </summary>
        public Gerenciador()
        {
            try
            {
                string conteudoQuartos = File.ReadAllText("../GerenciadorHotel/Arquivos/quartosRegistrados.json");
                QuartosRegistrados = JsonConvert.DeserializeObject<List<Quarto>>(conteudoQuartos);
                if (QuartosRegistrados == null || !QuartosRegistrados.Any())
                {
                    throw new Exception("Arquivo não encontrado");
                }
            } catch (Exception ex)
            {
                Console.WriteLine("ALERTA: arquivo sobre quartos não encontrado ou vazio ", ex.Message);
                QuartosRegistrados = new List<Quarto>();
            }

            try
            {
                string informacoesQuartos = File.ReadAllText("../GerenciadorHotel/Arquivos/reservas.json");
                Reservas = JsonConvert.DeserializeObject<List<ReservaGerenciador>>(informacoesQuartos);

                if (Reservas == null || !Reservas.Any())
                {
                    throw new Exception("Arquivo não encontrado");
                }
            } catch (Exception ex)
            {
                Console.WriteLine("ALERTA: arquivo de reservas não encontrado ou vazio ", ex.Message);
                Reservas = new List<ReservaGerenciador>();
            }

            LimparReservas();
        }
        public List<Quarto> QuartosRegistrados { get; set; }
        public List<ReservaGerenciador> Reservas { get; set; }

        /// <summary>
        /// Método utilizado para salvar os quartos do hotel.
        /// </summary>
        public void SalvarQuartos()
        {
            string serializado = JsonConvert.SerializeObject(QuartosRegistrados, Formatting.Indented);
            File.WriteAllText("Arquivos/quartosRegistrados.json", serializado);
        }

        /// <summary>
        /// Método utilizado para salvar as reservas.
        /// </summary>
        public void SalvarReservas()
        {
            string serializado = JsonConvert.SerializeObject(Reservas, Formatting.Indented);
            File.WriteAllText("../GerenciadorHotel/Arquivos/reservas.json", serializado);
        }

        /// <summary>
        /// Realiza a verificação e adição de quarto
        /// </summary>
        /// <param name="quarto"></param>
        public void AdicionarQuarto(Quarto quarto)
        {
            if (quarto.Capacidade > 0 && quarto.Tipo != null && quarto.ValorDiario > 0)
            {
                if (QuartosRegistrados.Any())
                {
                    quarto.Id = QuartosRegistrados.Last().Id + 1;
                }
                else
                {
                    quarto.Id = 1;
                }

                Console.WriteLine("Adicionando");
                QuartosRegistrados.Add(quarto);
            }
            else
            {
                Console.WriteLine("Formato inválido, a capacidade dele deve ser maior que zero, seu tipo precisa " +
                                  "ser um texto válido e seu custo diário deve ser maior que zero.");
            }
        }

        /// <summary>
        /// Lista os quartos registrados.
        /// </summary>
        /// <returns></returns>
        public bool ListarQuartos()
        {
            if (QuartosRegistrados.Count > 0)
            {
                Console.WriteLine("Os quartos registrados são:");

                foreach(Quarto quarto in QuartosRegistrados)
                {
                    Console.WriteLine($"Tipo: {quarto.Tipo}, capacidade: {quarto.Capacidade}, preço {quarto.ValorDiario}");
                }
                return true;
            }
            else 
            {
                Console.WriteLine("No momento não existem quartos registrados");
                return false;
            }
        }

        /// <summary>
        /// Remove um quarto, removendo junto todas as suas reservas.
        /// </summary>
        /// <param name="tipoSuite">O tipo do quarto que se deseja remover.</param>
        /// <param name="capacidade">A capacidade de hóspedes do quarto que se deseja remover.</param>
        /// <param name="precoDiario">O valor da diária do quarto que se deseja remover.</param>
        public void RemoverQuarto(string tipoSuite, int capacidade, decimal precoDiario)
        {
            (_, Quarto quartoASerRemovido) = EncontrarQuarto(tipoSuite, capacidade, precoDiario);

            if (QuartosRegistrados.Remove(quartoASerRemovido))
            {
                Console.WriteLine("Quarto removido com sucesso!");
                RemoverReservasDoQuarto(quartoASerRemovido);    
            }
            else
            {
                Console.WriteLine("Quarto não removido...");
            }
        }

        /// <summary>
        /// Remove todas as reservas de um determinado quarto.
        /// </summary>
        /// <param name="quarto">Quarto ao qual se deseja remover as reservas.</param>
        public void RemoverReservasDoQuarto(Quarto quarto)
        {
            for (int i = 0; i < Reservas.Count; i++)
            {
                if (Reservas[i].Id == quarto.Id)
                {
                    Reservas.Remove(Reservas[i]);
                }
            }
        }

        /// <summary>
        /// Confere que se um quarto está registrado
        /// </summary>
        /// <param name="tipoSuite">O tipo de suíte do quarto que se deseja verificar.</param>
        /// <param name="capacidade">A capacidade de hóspedes suportada do quarto que se deseja verificar.</param>
        /// <param name="precoDiario">O valor da diária do quarto que se deseja verificar.</param>
        /// <returns>Retorna um boolean se o quarto foi encontrado e seu objeto</returns>
        public (bool, Quarto) EncontrarQuarto(string tipoSuite, int capacidade, decimal precoDiario)
        {
            foreach(Quarto quarto in QuartosRegistrados)
            {
                if (tipoSuite == quarto.Tipo && capacidade == quarto.Capacidade && precoDiario == quarto.ValorDiario)
                {
                    return (true, quarto);
                }
            }
            Console.WriteLine("Quarto não encontrado");
            return (false, new Quarto());
        }

        /// <summary>
        /// Lista todas as reservas de um quarto.
        /// </summary>
        /// <param name="quarto">Quarto o qual se deseja listar as reservas.</param>
        /// <returns>Retorna um boolean que diz se existe pelo menos uma reserva no quarto ou não.</returns>
        public bool ListarReservas(Quarto quarto)
        {
            bool existeReserva = false;
            
            foreach(ReservaGerenciador reserva in Reservas)
            {
                if (quarto.Id == reserva.Id)
                {
                    Console.WriteLine($"Entrada: {reserva.Entrada.ToString("dd/MM/yyyy")}, saída: {reserva.Saida.ToString("dd/MM/yyyy")}, " + 
                                      $"hóspede responsável: {reserva.Hospedes[0].NomeCompleto}");
                    existeReserva = true;
                }
            }
            return existeReserva;
        }

        /// <summary>
        /// Gera um relatório sobre o hotel, descrevendo todos os quartos e listando suas reservas e lucros 
        /// </summary>
        /// <returns>Retorna uma string com todas as informações do relatório</returns>
        public string GerarRelatorio()
        {
            string relatorio = "--- Relatório do hotel ---";
            decimal lucroTotal = 0M;
            int quartosRegistrados = QuartosRegistrados.Count;

            if (quartosRegistrados > 1)
            {
                relatorio += $"\nExistem {quartosRegistrados} quartos!";
            } 
            else if (quartosRegistrados == 1)
            {
                relatorio += $"\nExiste 1 quarto registrado!";
            }
            else 
            {
                relatorio += $"\nNão existe quarto registrado!";
            }
            
            foreach(Quarto quarto in QuartosRegistrados)
            {
                int quantidadeDeReservas = 0;
                decimal lucroTotalDoQuarto = 0M;
                
                relatorio += $"\nO {quarto.Id}° é do tipo {quarto.Tipo}, ele suporta {quarto.Capacidade} pessoas e tem um custo diário de {quarto.ValorDiario.ToString("R$: 00.00")}.";
                
                foreach(ReservaGerenciador reserva in Reservas)
                {
                    if (reserva.Id == quarto.Id)
                    {
                        quantidadeDeReservas++;
                        if (quantidadeDeReservas == 1)
                        {
                            relatorio += "\nAs reservas ativas desse quarto são:";
                        }
                        lucroTotalDoQuarto += reserva.Valor;
                        relatorio += $"\nEntrada no dia {reserva.Entrada.ToString("dd/MM/yyyy")}, saída no dia {reserva.Saida.ToString("dd/MM/yyyy")}. Tendo um lucro de {reserva.Valor.ToString("R$: 00.00")}";
                        relatorio += "\nOs hóspedes são:";
                        foreach(Hospede hospede in reserva.Hospedes)
                        {
                            relatorio += $"\n - {hospede.NomeCompleto}";
                        }
                    }
                }
                relatorio += lucroTotalDoQuarto > 0 ? $"\nO lucro total desse quarto é de {lucroTotalDoQuarto.ToString("R$ 00.00")}": "";
                lucroTotal += lucroTotalDoQuarto;
            }
            relatorio += $"\nO lucro total do hotel será de {lucroTotal.ToString("R$ 00.00")}";
            Console.WriteLine(relatorio);
            return relatorio;
        }

        /// <summary>
        /// Salva o relatório.
        /// </summary>
        /// <param name="relatorio">Relatório a ser salvo</param>
        public void SalvarRelatorio(string relatorio)
        {
            File.WriteAllText("Arquivos/relatorio.txt", relatorio);
        }

        /// <summary>
        /// Remove reservas que já foram finalizadas (dia de saída menor que o dia atual).
        /// </summary>
        public void LimparReservas()
        {
            DateTime diaAtual = DateTime.Now;

            for (int i = 0; i < Reservas.Count; i++)
            {
                if (Reservas[i].Saida < diaAtual)
                {
                    Reservas.Remove(Reservas[i]);
                }
            }

            SalvarReservas();
        }

        /// <summary>
        /// Cancela uma reserva
        /// </summary>
        /// <param name="suite">Suíte a qual a reserva está cadastrada</param>
        /// <param name="entrada">Data de entrada da reserva</param>
        /// <param name="saida">Data de saída da reserva</param>
        /// <param name="nomeCompleto">Nome completo da pessoa responsável pela a reserva</param>
        public void CancelarReserva(Quarto suite, DateTime entrada, DateTime saida, string nomeCompleto)
        {
            bool reservaEncontrada = false;

            if (suite.Id < 1)
            {
                Console.WriteLine("Suíte não válida");
            }

            for (int i = 0; i < Reservas.Count; i++)
            {
                if (suite.Id == Reservas[i].Id && entrada == Reservas[i].Entrada && saida == Reservas[i].Saida 
                    && nomeCompleto == Reservas[i].Hospedes[0].NomeCompleto)
                {
                    if (Reservas.Remove(Reservas[i]))
                    {
                        Console.WriteLine("Reserva removida com sucesso!");
                        reservaEncontrada = true;
                        break;
                    }
                }
            }
            if (!reservaEncontrada)
                Console.WriteLine("Reserva não encontrada");
        }
    }
}