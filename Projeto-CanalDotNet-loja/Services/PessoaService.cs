using ProjetoCanalDotNetLoja.Models;

namespace ProjetoCanalDotNetLoja.Services
{
    public class PessoaService
    {
        public event Action OnChange;//Para notificar quando o usuario estiver online ou não

        private void NotifyStateChanged() => OnChange?.Invoke();
        private Peson _person;
        
        public Peson GetCurrentPeson()
        {
            return _person;
        }

        public void AtualizarPessoa(PersonLogin login)
        {
            if (login.FacePictures == null)
            {
                login.FacePictures = new FacePicture[0];//Cria-se uma primeira imagem
            }

            _person = new Peson()
            {
                Id = login.Id,
                Name = login.Name,
                FacePictures = login.FacePictures.Select(f => f.Base64Picture).ToArray()
            };

            NotifyStateChanged();// mostrando que o estatus mudou

        }

        public void Logout()
        {
            _person = null;
            NotifyStateChanged();
        }
    }
}
