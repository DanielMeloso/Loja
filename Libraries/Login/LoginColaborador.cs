using Loja.Models;
using Newtonsoft.Json;

namespace Loja.Libraries.Login
{
    public class LoginColaborador
    {
        private Sessao.Sessao _sessao;
        private string key = "Login.Colaborador";
        public LoginColaborador(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Colaborador colaborador)
        {
            var colaboradorJsonString = JsonConvert.SerializeObject(colaborador);
            _sessao.Cadastrar(key, colaboradorJsonString);
        }

        public Colaborador GetColaborador()
        {
            if (_sessao.Existe(key))
            {
                var colaboradorJsonString = _sessao.Consultar(key);
                return JsonConvert.DeserializeObject<Colaborador>(colaboradorJsonString);

            }
            return null;
        }

        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
