using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Lexico;

namespace Compilador.Exceptions
{
    /// <summary>
    /// Representa uma lista de erros ocorridos durante o processo de análise léxica.
    /// </summary>
    public class LexicoListException : Exception
    {
        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="lexema">O lexema que resultou em erro.</param>
        /// <param name="linha">A linha em que foi encontrado o erro.</param>
        /// <param name="posicao">A posição na linha em que foi encontrado o erro.</param>
        /// <param name="index">A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        public LexicoListException(List<LexicoError> listaErros) : base(listaErros.Count > 1 ? $"Foram encontrados {listaErros.Count} erros durante a análise léxica." : $"Foi encontrado {listaErros.Count} erro durante a análise léxica.")
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
        public LexicoListException(List<LexicoError> listaErros, string mensagem) : base(mensagem)
        {
            this.ListaErros = listaErros.ToArray();
        }

        /// <summary>
        /// Obtém uma lista de erros contendo as informações sobre os erros encontrados durante a análise léxica.
        /// </summary>
        public LexicoError[] ListaErros { get; }
    }
}
