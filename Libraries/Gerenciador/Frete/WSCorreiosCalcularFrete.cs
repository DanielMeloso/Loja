using Loja.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSCorreios;

namespace Loja.Libraries.Gerenciador.Frete
{
    public class WSCorreiosCalcularFrete
    {
        private IConfiguration _configuration;
        private CalcPrecoPrazoWSSoap _servico;

        public WSCorreiosCalcularFrete(IConfiguration configuration, CalcPrecoPrazoWSSoap servico)
        {
            _configuration = configuration;
            _servico = servico;
        }

        public async Task<ValorPrazoFrete> CalcularFrete(string cepDestino, string tipoFrete, List<Pacote> pacotes)
        {
            List<ValorPrazoFrete> valorDosPacotesPorFrete = new List<ValorPrazoFrete>();

            foreach(var pacote in pacotes)
            {
                var resultado = await CalcularValorPrazoFrete(cepDestino, tipoFrete, pacote);
                if (resultado != null)
                {
                    valorDosPacotesPorFrete.Add(resultado);
                }
            }

            if (valorDosPacotesPorFrete.Count > 0)
            {
                var valorDosFretes = valorDosPacotesPorFrete.GroupBy(x => x.TipoFrete)
                    .Select(list => new ValorPrazoFrete
                    {
                        TipoFrete = list.First().TipoFrete,
                        Prazo = list.Max(x => x.Prazo),
                        Valor = list.Sum(x => x.Valor)
                    }).ToList().First();

                return valorDosFretes;
            } else
            {
                return null;
            }
        }

        private async Task<ValorPrazoFrete> CalcularValorPrazoFrete(string cepDestino, string tipoFrete, Pacote pacote)
        {
            var cepOrigem = _configuration.GetValue<string>("Frete:CepOrigem");
            var maoPropria = _configuration.GetValue<string>("Frete:MaoPropria");
            var avisoRecebimento = _configuration.GetValue<string>("Frete:AvisoRecebimento");
            var diametro = Math.Max(Math.Max(pacote.Comprimento, pacote.Largura),pacote.Altura);
            cResultado resultado = await _servico.CalcPrecoPrazoAsync("","", tipoFrete, cepOrigem, cepDestino, pacote.Peso.ToString(), 1, pacote.Comprimento, pacote.Altura, pacote.Largura, diametro, maoPropria, 0, avisoRecebimento);

            if (resultado.Servicos[0].Erro == "0")
            {
                return new ValorPrazoFrete()
                {
                    TipoFrete = tipoFrete,
                    Prazo = int.Parse(resultado.Servicos[0].PrazoEntrega),
                    Valor = double.Parse(resultado.Servicos[0].Valor.Replace(".", "").Replace(",", "."))
                };
            }
            else if (resultado.Servicos[0].Erro == "008")
            {
                // Ex. SEDEX10 - Não disponível para o CEP informmado
                return null;
            } else
            {
                throw new Exception("Erro: " + resultado.Servicos[0].MsgErro);
            }
        }
    }
}
