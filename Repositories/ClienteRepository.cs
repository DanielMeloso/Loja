using Loja.Database;
using Loja.Models;
using Loja.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Loja.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private LojaContext _banco;
        public ClienteRepository(LojaContext banco)
        {
            _banco = banco;
        }
        public void Atualizar(Cliente cliente)
        {
            _banco.Update(cliente);
            _banco.SaveChanges();
        }

        public void Cadastrar(Cliente cliente)
        {
            _banco.Clientes.Add(cliente);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            var cliente = ObterCliente(Id);
            _banco.Remove(cliente);
            _banco.SaveChanges();
        }

        public Cliente Login(string Email, string Senha)
        {
            var cliente = _banco.Clientes.FirstOrDefault(x => x.Email == Email && x.Senha == Senha);
            return cliente;
        }

        public Cliente ObterCliente(int id)
        {
            return _banco.Clientes.Find(id);
        }

        public IEnumerable<Cliente> ObterTodosClientes()
        {
            return _banco.Clientes.ToList();
        }
    }
}
