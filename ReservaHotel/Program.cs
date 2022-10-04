using System.Text;
using ReservaHotel.Models;
using GerenciadorHotel.Models;

Console.OutputEncoding = Encoding.UTF8;

bool exibirMenu = true;
Reserva reserva = new Reserva();

reserva.Hospedes = new List<Pessoa>();
reserva.Suite = new Suite();

Console.WriteLine("Sistema de reserva de hotel");

while (exibirMenu)
{
    Console.WriteLine("Digite o número da sua opção");
    Console.WriteLine("1 - Adicionar hóspede");
    Console.WriteLine("2 - Remover hóspede");
    Console.WriteLine("3 - Listar hóspedes");
    Console.WriteLine("4 - Escolher suíte");
    Console.WriteLine("5 - Escolher a data de entrada e saída");
    Console.WriteLine("6 - Finalizar reserva");
    Console.WriteLine("7 - Sair do programa");

    switch (Console.ReadLine())
    {
        case "1":
            Console.WriteLine("Digite o primeiro nome da pessoa");
            string primeiroNome = Console.ReadLine();
            Console.WriteLine("Digite o sobrenome nome da pessoa");
            string sobrenome = Console.ReadLine();
            reserva.CadastrarHospedes(new Pessoa(primeiroNome, sobrenome));
            break;

        case "2":
            if (reserva.ObterQuantidadeHospedes() > 0)
            {
                reserva.ListarHospedes();
                Console.WriteLine("Digite o nome completo da pessoa");
                reserva.RemoverHospede(reserva.EncontrarHospede(Console.ReadLine()));
            }
            else
            {
                Console.WriteLine("Nenhum hóspede está cadastrado no momento");
            }
            break;

        case "3":
            reserva.ListarHospedes();
            break;

        case "4":
            Console.WriteLine("Temos essas suites disponíveis:");
            if(reserva.ListarSuites())
            {
                Console.WriteLine("Digite o tipo de suite que você quer");
                string tipoSuite = Console.ReadLine();
                Console.WriteLine("Digite a capacidade que você deseja");
                Int32.TryParse(Console.ReadLine(), out int capacidade);
                Console.WriteLine("Digite o custo diário que você prefere");
                Decimal.TryParse(Console.ReadLine(), out decimal valorDiario);
                reserva.EscolherSuite(new Suite(tipoSuite, capacidade, valorDiario));
            }
            break;

        case "5":
            if (reserva.Suite.Id == 0)
            {
                Console.WriteLine("Por favor, primeiro escolha uma suíte");
            }
            else
            {
                reserva.ListarDatasCadastradas();
                Console.WriteLine("Digite a data de entrada no formato dia, mês e ano, dessa forma: 25/09/2022");
                DateTime.TryParse(Console.ReadLine(), out DateTime entrada);
                Console.WriteLine("Digite a data de saida no formato dia, mês e ano, dessa forma: 25/09/2022");
                DateTime.TryParse(Console.ReadLine(), out DateTime saida);
                reserva.AdicionarDiasReservados(entrada, saida);
            }
            break;

        case "6":
            (bool sucesso, decimal custoTotal) = reserva.CalcularValorTotal();
            if (sucesso)
            {
                Console.WriteLine($"Valor total: R$ {custoTotal.ToString("00.00")}");
                exibirMenu = false;
            }
            else
            {
                Console.WriteLine("Por favor, corrija os problemas mencionados e volte para efetuar a reserva");
            }
            break;

        case "7":
            exibirMenu = false;
            break;

        default:
            Console.WriteLine("Opção inválida");
            break;
    }

    Console.WriteLine("Aperte enter para continuar");
    Console.ReadLine();
    Console.Clear();
}