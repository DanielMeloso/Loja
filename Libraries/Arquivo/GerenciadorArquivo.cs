using Loja.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Loja.Libraries.Arquivo
{
    public class GerenciadorArquivo
    {
        public static string CadastrarImagemProduto(IFormFile file)
        {
            var nomeArquivo = Path.GetFileName(file.FileName);
            var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", nomeArquivo);

            using (var stream = new FileStream(caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("/uploads/temp", nomeArquivo).Replace("\\","/");
        }

        public static bool DeletarImagemProduto(string caminho)
        {
            string caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", caminho.TrimStart('/'));
            if (File.Exists(caminhoArquivo))
            {
                File.Delete(caminhoArquivo);
                return true;
            }
            return false;
        }

        public static List<Imagem> MoverImagensProduto(List<string> listaCaminhoTemp, int produtoId)
        {
            // Criar pasta para o produto
            var caminhoDefinitivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", "prod-" + produtoId.ToString());

            if (!Directory.Exists(caminhoDefinitivo))
            {
                Directory.CreateDirectory(caminhoDefinitivo);
            }

            var listaImagensDef = new List<Imagem>();
            // Mover imagem da pasta Temp para a definitiva
            foreach(var caminhoTemp in listaCaminhoTemp)
            {
                if (!string.IsNullOrEmpty(caminhoTemp))
                {
                    var nomeArquivo = Path.GetFileName(caminhoTemp);
                    var caminhoDef = Path.Combine("/uploads", "prod-" + produtoId.ToString(), nomeArquivo).Replace("\\","/");
                    
                    if (caminhoDef != caminhoTemp)
                    {
                        var caminhoAbsolutoTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", nomeArquivo);
                        var caminhoAbsolutoDef = Path.Combine(caminhoDefinitivo, nomeArquivo);

                        if (File.Exists(caminhoAbsolutoTemp))
                        {
                            // Deleta arquivo no caminho de destino
                            if (File.Exists(caminhoAbsolutoDef))
                            {
                                File.Delete(caminhoAbsolutoDef);
                            }

                            // copia arquivo da pasta temp para destino
                            File.Copy(caminhoAbsolutoTemp, caminhoAbsolutoDef);

                            // Deletar arquivo da pasta temp
                            if (File.Exists(caminhoAbsolutoDef))
                            {
                                File.Delete(caminhoAbsolutoTemp);
                            }
                            listaImagensDef.Add(new Imagem(){ 
                                    Caminho = Path.Combine("/uploads", "prod-" + produtoId.ToString(), nomeArquivo).Replace("\\", "/"),
                                    ProdutoId = produtoId 
                                });
                        } else
                        {
                            return null;
                        }
                    } else
                    {
                        listaImagensDef.Add(new Imagem()
                        {
                            Caminho = Path.Combine("/uploads", "prod-" + produtoId.ToString(), nomeArquivo).Replace("\\", "/"),
                            ProdutoId = produtoId
                        });
                    }


                }
            }
            return listaImagensDef;
        }

        /// <summary>
        /// Deleta todas as imagens as imagens e a pasta do produto
        /// </summary>
        /// <param name="listaImagens">Lista de Imagens a deletar</param>
        public static void DeletarImagensProduto(List<Imagem> listaImagens)
        {
            int produtoId = 0;
            foreach(var imagem in listaImagens)
            {
                DeletarImagemProduto(imagem.Caminho);
                produtoId = imagem.ProdutoId;
            }

            var pastaProduto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", "prod-" + produtoId.ToString());
            if (Directory.Exists(pastaProduto))
            {
                Directory.Delete(pastaProduto);
            }
        }
    }
}
