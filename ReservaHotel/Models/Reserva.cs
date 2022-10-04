using Newtonsoft.Json;
using GerenciadorHotel.Models;

namespace ReservaHotel.Models
{
    /// <summary>
    /// Classe responsável por finalizar a reserva da suíte, ela engloba as outras classes, agrupando a suíte escolhida,
    /// uma lista dos hóspedes, as datas de entrada e saída e o custo final. Permite salvar essas informações no arquivo
    /// reservas e consome as informações de suítes do arquivo quartosRegistrados.
    /// </summary>
    public class Reserva
    {
        public List<Pessoa> Hospedes { get; set; }
        public Suite Suite { get; set; }
        public List<Suite> SuitesDisponiveis = new List<Suite>();
        public (DateTime entrada, DateTime saida) DiasReservados { get; set; }
        public List<DadosReserva> Reservas { get; set; }

        /// <summary>
        /// Construtor da classe, ele inicia as listas de dados e suítes além de atribuir as informações existentes nelas.
        /// </summary>
        public Reserva()
        {
            Reservas = new List<DadosReserva>();
            SuitesDisponiveis = new List<Suite>();

            try
            {
                string conteudoArquivo = File.ReadAllText("../GerenciadorHotel/Arquivos/quartosRegistrados.json");
                SuitesDisponiveis = JsonConvert.DeserializeObject<List<Suite>>(conteudoArquivo);

                if (SuitesDisponiveis == null || !SuitesDisponiveis.Any())
                    {
                        SuitesDisponiveis = new List<Suite>();
                    }
            } catch (Exception ex)
            {
                Console.WriteLine("Erro ao receber as suítes, " + ex.Message);
                SuitesDisponiveis = new List<Suite>();
            }

            try
            {
                string counteudoDosDados = File.ReadAllText("../GerenciadorHotel/Arquivos/reservas.json");
                Reservas = JsonConvert.DeserializeObject<List<DadosReserva>>(counteudoDosDados);

                if (Reservas == null || !Reservas.Any())
                {
                    Reservas = new List<DadosReserva>();
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Erro ao receber os dados, " + ex.Message);
                Reservas = new List<DadosReserva>(); 
            }
        }

        /// <summary>
        /// Método que serializa a propriedade Reservas e grava no arquivo reservas.
        /// </summary>
        public void SalvarArquivo()
        {
            string serializacao = JsonConvert.SerializeObject(Reservas, Formatting.Indented);
            File.WriteAllText("../GerenciadorHotel/Arquivos/reservas.json", serializacao);
        }

        /// <summary>
        /// Método que adiciona hóspedes
        /// </summary>
        /// <param name="hospede">Argumento que representa a pessoa que se deseja adicionar</param>
        public void CadastrarHospedes(Pessoa hospede)
        {
            try
            {
                if (ObterQuantidadeHospedes() + 1 <= Suite.Capacidade
                    || Suite.Capacidade == 0)
                {
                    if(!Hospedes.Exists(h => h.NomeCompleto == hospede.NomeCompleto))
                    {
                        Hospedes.Add(hospede);
                    }
                    else
                    {
                        Console.WriteLine("Hóspede já cadastrado");
                    }
                }
                else
                {
                    throw new Exception("A quantidade de hóspedes deve ser menor ou igual à capacidade da suíte.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Número de hóspedes maior que a capacidade da suíte, por favor, escolha uma maior.");
            }
        }

        /// <summary>
        /// Método que permite selecionar uma suíte.
        /// </summary>
        /// <param name="suite">Suíte a ser selecionada</param>
        public void EscolherSuite(Suite suite)
        {
            (bool encontrou, Suite suiteEncontrada) resultado = ConferirSuite(suite);
            if (resultado.encontrou)
            {
                // Limpa os dias caso a suite seja trocada
                DiasReservados = (new DateTime(), new DateTime());
                Suite = resultado.suiteEncontrada;
            }
        }

        /// <summary>
        /// Retorna a quantidade de hóspedes registrados no momento da reserva
        /// </summary>
        /// <returns>Quantidade de hóspedes registrados</returns>
        public int ObterQuantidadeHospedes()
        {
            int quantidadeHospedes = 0;

            // Evita exceção caso não tenha nenhum hóspede já adicionado
            if (Hospedes != null)
            {
                quantidadeHospedes = Hospedes.Count;
            }

            return quantidadeHospedes;
        }

        /// <summary>
        /// Verifica se a reserva pode ser finalizada e calcula o valor total
        /// </summary>
        /// <returns>Retorna uma tupla, o primeiro valor é um bool dizendo se deu certo, o segundo é o custo total da reserva
        /// (decimal)</returns>
        public (bool valoresValidos, decimal custoTotal) CalcularValorTotal()
        {
            if (VerificarDadosDeReserva())
            {
                int quantidadeDeDias = DiasReservados.saida.Subtract(DiasReservados.entrada).Days;
                decimal valor = quantidadeDeDias * Suite.ValorDiaria;

                if (quantidadeDeDias >= 10)
                {
                    decimal desconto = 0.1M;
                    valor -= valor * desconto;
                }
                
                Reservas.Add(new DadosReserva(DiasReservados.entrada, DiasReservados.saida, Hospedes, valor, Suite.Id));
                SalvarArquivo();
                return (true, valor);
            }
            return (false, 0);
        }

        /// <summary>
        /// Verifica se todos os dados da reserva, como quantidade de hóspede mínima, dias de entrada e saída diferentes e id
        /// da suíte estão corretos.
        /// </summary>
        /// <returns>Retorna um boolean dizendo se os dados estão corretos</returns>
        public bool VerificarDadosDeReserva()
        {
            bool reservaEstaCorreta = true;
            if (ObterQuantidadeHospedes() < 1)
            {
                reservaEstaCorreta = false;
                Console.WriteLine("A quantidade de hóspedes deve ser no mínimo 1");
            }
            else if (DiasReservados.entrada == DiasReservados.saida)
            {
                reservaEstaCorreta = false;
                Console.WriteLine("O dia de entrada e de saída devem ser diferentes");
            }
            else if (Suite.Id == 0)
            {
                reservaEstaCorreta = false;
                Console.WriteLine("Precisa escolher uma suíte válida");
            }
            return reservaEstaCorreta;
        }

        /// <summary>
        /// Lista todas as suítes disponíveis
        /// </summary>
        /// <returns>Retorna se pelo menos uma suíte foi encontrada</returns>
        public bool ListarSuites()
        {
            bool encontrouSuite = false;
            foreach (Suite suite in SuitesDisponiveis)
            {
                if (suite.Capacidade >= ObterQuantidadeHospedes())
                {
                    Console.WriteLine($"Capacidade: {suite.Capacidade}, Tipo: {suite.TipoSuite}, Valor: R$ {suite.ValorDiaria.ToString("00.00")}");
                    encontrouSuite = true;
                }
            }

            if (!encontrouSuite)
                Console.WriteLine("Nenhuma suíte encontrada");

            return encontrouSuite;
        }

        /// <summary>
        /// Confere se a suíte passada é uma suíte válida, listada
        /// </summary>
        /// <param name="suiteASerTestada">Suite que se deseja conferir</param>
        /// <returns></returns>
        public (bool, Suite) ConferirSuite(Suite suiteASerTestada)
        {
            foreach (Suite suiteAtual in SuitesDisponiveis)
            {
                if (suiteAtual.TipoSuite == suiteASerTestada.TipoSuite &&
                    suiteAtual.Capacidade == suiteASerTestada.Capacidade &&
                    suiteAtual.ValorDiaria == suiteASerTestada.ValorDiaria)
                {
                    Console.WriteLine("Suíte Encontrada!");
                    return (true, suiteAtual);
                }
            }
            Console.WriteLine("Suíte não encontrada");
            return (false, new Suite());
        }

        /// <summary>
        /// Lista os hóspedes registrados
        /// </summary>
        public void ListarHospedes()
        {
            int quantidadeDeHospedes = ObterQuantidadeHospedes();
            if (quantidadeDeHospedes > 0)
            {
                Console.WriteLine(quantidadeDeHospedes > 1 ? $"Atualmente {quantidadeDeHospedes} hóspedes estão cadastrados, sendo eles:" : 
                                  $"Atualmente {quantidadeDeHospedes} hóspede está cadastrado, sendo ele");

                foreach (Pessoa hospede in Hospedes)
                {
                    Console.WriteLine(hospede.NomeCompleto);
                }
            }
            else 
            {
                Console.WriteLine("No momento não existem hóspedes cadastrados");
            }
        }

        /// <summary>
        /// Pesquisa uma pessoa através de seu nome comlpeto, retornando seu objeto Pessoa
        /// </summary>
        /// <param name="nomeCompleto">Nome completo da pessoa que se quer encontrar</param>
        /// <returns>Retorna o objeto Pessoa encontrado</returns>
        public Pessoa EncontrarHospede(string nomeCompleto)
        {
            Pessoa pessoa = new Pessoa();
            for (int i = 0; i < Hospedes.Count; i++)
            {
                if (Hospedes[i].NomeCompleto == nomeCompleto)
                {
                    pessoa = Hospedes[i];
                    Console.WriteLine("Hóspede encontrado!");
                    break;
                }
            }
            return pessoa;
        }

        /// <summary>
        /// Remove hóspede
        /// </summary>
        /// <param name="hospede">Pessoa a ser removida</param>
        public void RemoverHospede(Pessoa hospede)
        {
            if (Hospedes.Remove(hospede))
            {
                Console.WriteLine("Removido com sucesso!");
            }
            else
            {
                Console.WriteLine("Hóspede não encontrado, por favor, tente novamente");
            }
        }

        /// <summary>
        /// Seleciona os dias desejados para permanecer na suíte
        /// </summary>
        /// <param name="entrada">Dia de início da reserva</param>
        /// <param name="saida">Dia final da reserva</param>
        public void AdicionarDiasReservados(DateTime entrada, DateTime saida) //Deixar só reservar um único dia, caso registre outro, trocar com o reservado
        {
            if (VerificarDisponibilidadeDeDatas(entrada, saida, Suite.Id)) 
            {
                DiasReservados = (entrada, saida);
                Console.WriteLine("Datas cadastradas com sucesso!");
            }
            else
            {
                Console.WriteLine("Data conflituosa, escolha outra por favor");
            }
        }

        /// <summary>
        /// Verifica a disponibilidade das datas, de acordo com a suíte
        /// </summary>
        /// <param name="entrada">Dia de início da reserva</param>
        /// <param name="saida">Dia final da reserva</param>
        /// <param name="id">Id referente à suíte</param>
        /// <returns>Retorna um booleano se as datas estão disponíveis ou não na suíte</returns>
        public bool VerificarDisponibilidadeDeDatas(DateTime entrada, DateTime saida, int id)
        {
            DateTime dataAtual = DateTime.Now;
            if ((entrada.Day >= dataAtual.Day || entrada.Month > dataAtual.Month || entrada.Year > dataAtual.Year) 
                && saida > entrada)
            {
                foreach (DadosReserva dado in Reservas)
                {
                    if (dado.Id != id)
                    {
                        continue;
                    }
                    
                    else if (dado.Entrada <= entrada && saida <= dado.Saida ||
                            (entrada <= dado.Saida && saida >= dado.Saida) ||
                            (entrada <= dado.Entrada && saida >= dado.Entrada))
                    {
                        Console.WriteLine("A data de entrada e saída não pode estar entre as já reservadas, " +
                                          "também não podem englobar outras datas já reservadas. Por favor, " +
                                          "escolha outra");
                        return false;
                    }
                }
                return true;
            }
            Console.WriteLine("A data de entrada deve ser maior ou igual a de hoje.\nA data de saída, deve " +
                              "ser maior que a de entrada");
            return false;
        }

        /// <summary>
        /// Lista todas as datas já cadastradas, verificando se o id corresponde ao da suíte já adicionada
        /// </summary>
        public void ListarDatasCadastradas()
        {
            string texto = "";
            bool existeDataCadastrada = false;
            foreach (DadosReserva dado in Reservas)
            {
                if (dado.Id == Suite.Id)
                {
                    if (!existeDataCadastrada)
                    {
                        texto += "Temos essa(s) data(s) registrada(s) para essa suíte";
                        existeDataCadastrada = true;
                    }
                    texto += $"\nEntrada: {dado.Entrada.ToString("dd/MM/yyyy")}, saída: {dado.Saida.ToString("dd/MM/yyyy")}";
                }
            }

            if(Suite.Id == 0)
            {
                texto = "Por favor, selecione uma suíte!";
            }
            else if (!existeDataCadastrada)
            {
                texto = "Não existem datas registradas para essa suíte";
            }

            Console.WriteLine(texto);
        }
    }
}