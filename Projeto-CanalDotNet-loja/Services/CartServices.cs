using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;//Aqui é usado o HttpClient para as rewuisições
using ProjetoCanalDotNetLoja.Models;
using System.Runtime.InteropServices;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ProjetoCanalDotNetLoja.Services
{
    public class CartServices
{
        private readonly HttpClient _httpClient;
        private Guid _session;
     
        private readonly string _baseUrl;


            public event Action OnChange;
        //Construtor da classe para fazer as requisições de uma forma mais inteigente
            public CartServices(HttpClient httpClient, IConfiguration configuration)
            {
            _httpClient = httpClient;
            _baseUrl = configuration.GetValue<string>("BaseUrl");

        }
        //Me mostra se o stado da notificação foi alterado
        private void NotifayStateChanged()=> OnChange?.Invoke();
        
        //Obtem o ID da sessão atual ou nova
        public  Guid GetCurrenteSessionIdOrNew()
        {
            if (_session == Guid.Empty)
            {
                _session = Guid.NewGuid();
            }
            return _session;

        }

        //Obter carrinho
        public async Task<Carrinho> GetCarrinho()
        {
            //Verificar com Eduardo aonde tem esse Link, para q eu possa ajustar no arquivo "appsetings"
            var sessionId = GetCurrenteSessionIdOrNew();
            var url = $"{_baseUrl}/carrinho/{sessionId}";
            var resposta = await _httpClient.GetFromJsonAsync<CarrinhoResposta>(url);
            //Tudo isso criando anteriormente deveria ser criado em outra classe separada, para que fosse chamada de configuração

           
            
            return resposta.Data ?? new Carrinho() {IdSessao = sessionId };

        }


        //Adicionar Item
        public async Task<Carrinho> AdicionaItem(ItemDeCompra item)
        {
            var sessionId =  GetCurrenteSessionIdOrNew();
            //Perguntar a eduardo de onde vem esse Link
            var url = $"{_baseUrl}/carrinho/{sessionId}";
            var data = new
            {
                IdProduto = item.Produto.Id.ToString(),
            Quantidade = item.Quantidade
            };
            await _httpClient.PostAsJsonAsync(url,data);

            NotifayStateChanged();//Notificação do status do cliente
            return await GetCarrinho(); //Busca no metodo quem é e o que tem, para que posa ser Atualizado todos o escopo do carrinho
        }

        //Atualizar Item do Carrinho
        public async Task<Carrinho> AtualizarItem(ItemDeCompra item)
        {
            var sessionId = GetCurrenteSessionIdOrNew();
            //Perguntar a eduardo de onde vem esse Link
            var url = $"{_baseUrl}/carrinho/{sessionId}";
            var data = new
            {
                IdProduto = item.Produto.Id.ToString(),
                Quantidade = item.Quantidade
            };
            await _httpClient.PatchAsync(url,new StringContent(JsonSerializer.Serialize(data)));

            NotifayStateChanged();//Notificação do status do cliente
            return await GetCarrinho(); //Busca no metodo quem é e o que tem, para que posa ser Atualizado todos o escopo do carrinho
        }
        

        //Remover Item do Carrinho
        public async Task<Carrinho> RemoverItem(ItemDeCompra item)
        {
            var sessionId = GetCurrenteSessionIdOrNew();
            var idPrtoduto = item.Produto.Id;
            //Perguntar a eduardo de onde vem esse Link
            var url = $"{_baseUrl}/carrinho/{sessionId}/{idPrtoduto}";//Aqui elimina apenas o produto
          
            NotifayStateChanged();//Notificação do status do cliente
            return await GetCarrinho(); //Busca no metodo quem é e o que tem, para que posa ser Atualizado todos o escopo do carrinho
        }

        //Limpar sessão de todo o carrinho
        public async Task<Carrinho> limparCarrinho()
        {
            var sessionId = GetCurrenteSessionIdOrNew();
            //Perguntar a eduardo de onde vem esse Link
            var url = $"{_baseUrl}/carrinho/{sessionId}";//aqui ele Limpa toda a Sessão, e não importa a quantidade de produtos
            
            await _httpClient.DeleteAsync(url);

            NotifayStateChanged();//Notificação do status do cliente
            return await GetCarrinho(); //Busca no metodo quem é e o que tem, para que posa ser Atualizado todos o escopo do carrinho
        }
    }


        
}

