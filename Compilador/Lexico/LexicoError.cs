using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Lexico
{
    /// <summary>
    /// Representa um erro ocoorido durante o processo de análise léxica.
    /// </summary>
    public class LexicoError
    {
        #region Constructors
        
        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="lexema">O lexema que resultou em erro.</param>
        /// <param name="linha">A linha em que foi encontrado o erro.</param>
        /// <param name="posicao">A posição na linha em que foi encontrado o erro.</param>
        /// <param name="index">A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        public LexicoError(string lexema, int linha, int posicao, int index)
        {
            this.Lexema = lexema;
            this.Linha = linha;
            this.Posição = posicao;
            this.Index = index - lexema.Length + 1;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Obtém o lexema que resultou em erro.
        /// </summary>
        public string Lexema { get; }

        /// <summary>
        /// Obtém a linha com o erro.
        /// </summary>
        public int Linha { get; }

        /// <summary>
        /// Obtém a posição na linha em que foi encontrado o erro.
        /// </summary>
        public int Posição { get; }

        /// <summary>
        /// Obtém a posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.
        /// </summary>
        public int Index { get; set; }

        #endregion
        
        /// <summary>
        /// Retorna um texto contendo as informações do token armazenado.
        /// </summary>
        public override string ToString()
        {
            return $"Erro léxico no lexema: '{this.Lexema}', Linha: {this.Linha}, Posição: {this.Posição}";
        }
    }
}
