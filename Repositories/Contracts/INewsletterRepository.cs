using Loja.Models;
using System.Collections.Generic;

namespace Loja.Repositories.Contracts
{
    public interface INewsletterRepository
    {
        void Cadastrar(NewsletterEmail newsletter);
        IEnumerable<NewsletterEmail> ObterTodasNewsletter();
    }
}
