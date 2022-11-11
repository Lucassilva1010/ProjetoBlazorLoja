namespace ProjetoCanalDotNetLoja.Models
{
    public class Produto
    {
        public Produto()
        {
            Preco = 0m;
            Fotos = new List<string>();

        }
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public List<string>Fotos { get; set; }
    }
}
