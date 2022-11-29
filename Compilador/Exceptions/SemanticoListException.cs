using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Semantico;

namespace Compilador.Exceptions
{
    /// <summary>
    /// Representa uma lista de erros ocorridos durante o processo de análise semântica.
    /// </summary>
    public class SemanticoListException : Exception
    {
        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="lexema">O lexema que resultou em erro.</param>
        /// <param name="linha">A linha em que foi encontrado o erro.</param>
        /// <param name="posicao">A posição na linha em que foi encontrado o erro.</param>
        /// <param name="index">A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        public SemanticoListException(List<SemanticoError> listaErros) : base(listaErros.Count > 1 ? $"Foram encontrados {listaErros.Count} erros durante a análise semântica." : $"Foi encontrado {listaErros.Count} erro durante a análise semântica.")
        {
            this.ListaErros = listaErros.ToArray();
        }

        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="lexema">O lexema que resultou em erro.</param>
        /// <param name="linha">A linha em que foi encontrado o erro.</param>
        /// <param name="posicao">A posição na linha em que foi encontrado o erro.</param>
        /// <param name="index">A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        public SemanticoListException(List<SemanticoError> listaErros, string mensagem) : base(mensagem)
        {
            this.ListaErros = listaErros.ToArray();
        }

        /// <summary>
        /// Obtém uma lista de erros contendo as informações sobre os erros encontrados durante a análise léxica.
        /// </summary>
        public SemanticoError[] ListaErros { get; }

        public string Erros()
        {
            string erros = String.Empty;

            for (int i = 0; i < this.ListaErros.Length; i++)
            {
                erros += $"* {this.ListaErros[i]}{Environment.NewLine}";

                if (i < this.ListaErros.Length - 1)
                    erros += Environment.NewLine;
            }

            return erros;
        }
    }
}
