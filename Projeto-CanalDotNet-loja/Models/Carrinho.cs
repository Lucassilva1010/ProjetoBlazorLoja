using System.Reflection.Metadata.Ecma335;

namespace ProjetoCanalDotNetLoja.Models
{
    public class Carrinho
    {
        public Guid IdSessao { get; set; }
        public List<ItemDeCompra> ItensDeCompra { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotalDoCarrinho { get; set; }

        public Carrinho()
        {
            ItensDeCompra = new List<ItemDeCompra>();
        }

        public void AdicionarIntem(ItemDeCompra item)
        {
     
            //Verifica se existe algum produto selecionado - ANY
            if (!ItensDeCompra.Any(i => i.Produto.Id == item.Produto.Id))
            {
                ItensDeCompra.Add(item);
            }
            else
            {
                //Me retonar o primeiro produto da sequencia- First
                var itemExistente = ItensDeCompra.First( i => i.Produto.Id == item.Produto.Id);
                itemExistente.Quantidade = +item.Quantidade;
            }
        }
    }
}
