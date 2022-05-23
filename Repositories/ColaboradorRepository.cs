using Loja.Database;
using Loja.Models;
using Loja.Models.Constants;
using Loja.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Loja.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly IConfiguration _configuration;
        private LojaContext _banco;
        public ColaboradorRepository(LojaContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _configuration = configuration;
        }

        public void Atualizar(Colaborador colaborador)
        {
            _banco.Update(colaborador);
            _banco.Entry(colaborador).Property(x => x.Senha).IsModified = false;
            _banco.SaveChanges();
        }

        public void AtualizarSenha(Colaborador colaborador)
        {
            _banco.Update(colaborador);
            _banco.Entry(colaborador).Property(x => x.Nome).IsModified = false;
            _banco.Entry(colaborador).Property(x => x.Email).IsModified = false;
            _banco.Entry(colaborador).Property(x => x.Tipo).IsModified = false;
            _banco.SaveChanges();
        }

        public void Cadastrar(Colaborador colaborador)
        {
            _banco.Add(colaborador);
            _banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            Colaborador colaborador = ObterColaborador(id);
            _banco.Remove(colaborador);
            _banco.SaveChanges();
        }

        public Colaborador Login(string Email, string Senha)
        {
            var colaborador = _banco.Colaboradores.FirstOrDefault(x => x.Email == Email && x.Senha == Senha);
            return colaborador;
        }

        public Colaborador ObterColaborador(int id)
        {
            return _banco.Colaboradores.Find(id);
        }

        public List<Colaborador> ObterColaboradorPorEmail(string email)
        {
            return _banco.Colaboradores.Where(x => x.Email == email).AsNoTracking().ToList();
        }

        public IPagedList<Colaborador> ObterTodosColaboradores(int? pagina)
        {
            int NumeroPagina = pagina ?? 1;
            return _banco.Colaboradores.Where(x => x.Tipo != ColaboradorTipoConstant.GERENTE).ToPagedList(NumeroPagina, _configuration.GetValue<int>("RegistroPorPagina"));
        }
    }
}
