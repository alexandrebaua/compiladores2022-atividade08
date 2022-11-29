using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Lexico;

namespace Compilador.Exceptions
{
    /// <summary>
    /// Representa um erro ocoorido durante o processo de análise sintática.
    /// </summary>
    public class SintaticoException : Exception
    {
        #region Constructors

        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="token">O token que resultou em erro.</param>
        public SintaticoException(TokenClass token) : this(token, $"Erro sintático no lexema: '{token.Lexema}'{Environment.NewLine}Linha: {token.Linha}, Posição: {token.Inicio}") { }

        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="token">O token que resultou em erro.</param>
        /// <param name="mensagem">Uma mensagem que descreve o erro.</param>
        public SintaticoException(TokenClass token, string mensagem) : base(mensagem)
        {
            this.Token = token;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// O token que resultou em erro
        /// </summary>
        public TokenClass Token { get; }

        #endregion
    }
}
