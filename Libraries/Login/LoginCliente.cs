using Loja.Models;
using Newtonsoft.Json;

namespace Loja.Libraries.Login
{
    public class LoginCliente
    {
        private Sessao.Sessao _sessao;
        private string key = "Login.Cliente";
        public LoginCliente(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Cliente cliente)
        {
            var clienteJsonString = JsonConvert.SerializeObject(cliente);
            _sessao.Cadastrar(key, clienteJsonString);
        }

        public Cliente GetCliente()
        {
            if (_sessao.Existe(key))
            {
                var clienteJsonString = _sessao.Consultar(key);
                return JsonConvert.DeserializeObject<Cliente>(clienteJsonString);

            }
            return null;
        }

        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
