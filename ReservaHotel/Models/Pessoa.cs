namespace ReservaHotel.Models
{
    /// <summary>
    /// Classe responsável por representar uma pessoa, tendo as informações de nome, sobrenome e nome completo
    /// </summary>
    public class Pessoa
    {
        public Pessoa() { }

        public Pessoa(string nome)
        {
            Nome = nome;
        }

        public Pessoa(string nome, string sobrenome)
        {
            Nome = nome;
            Sobrenome = sobrenome;
        }

        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeCompleto => $"{Nome} {Sobrenome}";
    }
}