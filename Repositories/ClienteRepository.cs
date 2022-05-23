using Loja.Database;
using Loja.Models;
using Loja.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Loja.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private LojaContext _banco;
        private IConfiguration _configuration;
        public ClienteRepository(LojaContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _configuration = configuration;
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

        public IPagedList<Cliente> ObterTodosClientes(int? pagina, string pesquisa)
        {
            int NumeroPagina = pagina ?? 1;
            var bancoCLiente = _banco.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                bancoCLiente = bancoCLiente.Where(x => x.Nome.Contains(pesquisa.Trim()) || x.Email.Contains(pesquisa.Trim()));
            }

            return bancoCLiente.ToPagedList(NumeroPagina, _configuration.GetValue<int>("RegistroPorPagina"));
        }
    }
}
