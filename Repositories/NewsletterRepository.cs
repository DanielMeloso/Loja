using Loja.Database;
using Loja.Models;
using Loja.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Loja.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private LojaContext _banco;
        public NewsletterRepository(LojaContext banco)
        {
            _banco = banco;
        }

        public void Cadastrar(NewsletterEmail newsletter)
        {
            _banco.NewsletterEmails.Add(newsletter);
            _banco.SaveChanges();
        }

        public IEnumerable<NewsletterEmail> ObterTodasNewsletter()
        {
            return _banco.NewsletterEmails.ToList();
        }
    }
}
