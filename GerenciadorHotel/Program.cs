using GerenciadorHotel.Models;

Console.WriteLine("Bem vindo ao sistema de gerenciador de Hotel");
Console.WriteLine("Por favor, digite o número da sua opção!");

bool exibirMenu = true;
Gerenciador banco = new Gerenciador();
List<Quarto> quartos = new List<Quarto>();
ReservaGerenciador reserva = new ReservaGerenciador();

while (exibirMenu)
{
    Quarto quarto = new Quarto();
    Console.WriteLine("1 - Adicionar quarto");
    Console.WriteLine("2 - Remover quarto");
    Console.WriteLine("3 - Cancelar reserva");
    Console.WriteLine("4 - Gerar relatório");
    Console.WriteLine("5 - Salvar e sair");
    Console.WriteLine("6 - Sair");

    string escolha = Console.ReadLine();

    switch(escolha)
    {
        case "1":
            quarto.AtribuirInformacoes();
            banco.AdicionarQuarto(quarto);
            break;

        case "2":
            if (banco.ListarQuartos())
            {
                quarto.AtribuirInformacoes();
                banco.RemoverQuarto(quarto.Tipo, quarto.Capacidade, quarto.ValorDiario);
            }
            break;

        case "3":
            if (banco.ListarQuartos())
            {
                (string tipo, int capacidade, decimal custoDiaria) = quarto.AdicionarInformacoes();
                (bool encontrou, Quarto suite) = banco.EncontrarQuarto(tipo, capacidade, custoDiaria);
                if (encontrou)
                {
                    if (banco.ListarReservas(suite))
                    {
                        Console.WriteLine("Digite  a data de entrada, seguindo o padrão dia, mês e ano separados por barra. Por exemplo, 25/09/2022");
                        DateTime.TryParse(Console.ReadLine(), out DateTime diaEntrada);
                        Console.WriteLine("Digite  a data de saída, seguindo o padrão dia, mês e ano separados por barra. Por exemplo, 25/09/2022");
                        DateTime.TryParse(Console.ReadLine(), out DateTime diaSaida);
                        Console.WriteLine("Digite o nome completo do responsável");
                        string nomeCompleto = Console.ReadLine();

                        banco.CancelarReserva(suite, diaEntrada, diaSaida, nomeCompleto);
                    }
                    else
                    {
                        Console.WriteLine("Não existe reserva nesse quarto");
                    }
                }
                else
                {
                    Console.WriteLine("Quarto não encontrado");
                }
            }
            break;

        case "4":
            banco.SalvarRelatorio(banco.GerarRelatorio());
            Console.WriteLine("O relatório está disponível na pasta Arquivos");
            break;

        case "5":
            banco.SalvarQuartos();
            banco.SalvarReservas();
            exibirMenu = false;
            break;

        case "6":
            exibirMenu = false;
            break;

        default:
            Console.WriteLine("Opção inválida");
            break;
    }
    Console.WriteLine("Tecle enter para continuar");
    Console.ReadLine();
    Console.Clear();
}