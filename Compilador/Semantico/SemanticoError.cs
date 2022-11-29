using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Semantico.Tipos;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico
{
    /// <summary>
    /// Representa um erro ocoorido durante o processo de análise semântica.
    /// </summary>
    public class SemanticoError
    {
        #region Constructors

        /// <summary>
        /// Cria uma exceção usando as informações da chamada de procedimento passada.
        /// </summary>
        /// <param name="procedimento">A chamada responsável pelo erro.</param>
        /// <param name="informação">Informação complementar sobre o erro.</param>
        public SemanticoError(ChamaProcedimento procedimento, string informação) : this(procedimento)
        {
            this.Informação = informação;
        }

        /// <summary>
        /// Cria uma exceção usando as informações da chamada de procedimento passada.
        /// </summary>
        /// <param name="procedimento">A chamada responsável pelo erro.</param>
        public SemanticoError(ChamaProcedimento procedimento) : this(procedimento.Nome.Lexema, procedimento.Elementos[0].Linha, procedimento.Elementos[0].Inicio, procedimento.Elementos[0].Index) { }

        /// <summary>
        /// Cria uma exceção usando as informações do token semântico passado.
        /// </summary>
        /// <param name="token">O token relacionado ao erro.</param>
        /// <param name="informação">Informação complementar sobre o erro.</param>
        public SemanticoError(SemanticoToken token, string informação) : this(token)
        {
            this.Informação = informação;
        }

        /// <summary>
        /// Cria uma exceção usando as informações do token semântico passado.
        /// </summary>
        /// <param name="token">O token relacionado ao erro.</param>
        public SemanticoError(SemanticoToken token) : this(token.Lexema, token.Linha, token.Inicio, token.Index) { }

        /// <summary>
        /// Cria uma exceção usando as informações da variável passada.
        /// </summary>
        /// <param name="variavel">A variável responsável pelo erro.</param>
        /// <param name="informação">Informação complementar sobre o erro.</param>
        public SemanticoError(Variável variavel, string informação) : this(variavel)
        {
            this.Informação = informação;
        }

        /// <summary>
        /// Cria uma exceção usando as informações da variável passada.
        /// </summary>
        /// <param name="variavel">A variável responsável pelo erro.</param>
        public SemanticoError(Variável variavel) : this(variavel.Token.Lexema, variavel.Token.Linha, variavel.Token.Inicio, variavel.Token.Index) { }

        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="lexema">O lexema que resultou em erro.</param>
        /// <param name="linha">A linha em que foi encontrado o erro.</param>
        /// <param name="posicao">A posição na linha em que foi encontrado o erro.</param>
        /// <param name="index">A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        /// <param name="comprimento">O comprimento da seleção necessária para o erro no texto.</param>
        public SemanticoError(string lexema, int linha, int posicao, int index) : this(lexema, linha, posicao, index, lexema.Length) { }

        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="lexema">O lexema que resultou em erro.</param>
        /// <param name="linha">A linha em que foi encontrado o erro.</param>
        /// <param name="posicao">A posição na linha em que foi encontrado o erro.</param>
        /// <param name="index">A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        /// <param name="comprimento">O comprimento da seleção necessária para o erro no texto.</param>
        public SemanticoError(string lexema, int linha, int posicao, int index, int comprimento) : this(lexema, linha, posicao, index, lexema.Length, null) { }

        /// <summary>
        /// Cria uma exceção contendo as informações passadas.
        /// </summary>
        /// <param name="lexema">O lexema que resultou em erro.</param>
        /// <param name="linha">A linha em que foi encontrado o erro.</param>
        /// <param name="posicao">A posição na linha em que foi encontrado o erro.</param>
        /// <param name="index">A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        /// <param name="comprimento">O comprimento da seleção necessária para o erro no texto.</param>
        /// <param name="informação">Informação complementar sobre o erro.</param>
        public SemanticoError(string lexema, int linha, int posicao, int index, int comprimento, string informação)
        {
            this.Lexema = lexema;
            this.Linha = linha;
            this.Posição = posicao;
            //this.Index = index - lexema.Length + 1;
            this.Index = index;
            this.Comprimento = comprimento;

            if (informação != null)
                this.Informação = informação;
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
        public int Index { get; }

        /// <summary>
        /// Obtém o comprimento da seleção necessária para o erro no texto.
        /// </summary>
        public int Comprimento { get; }

        /// <summary>
        /// Obtém ou define alguma informação complementar sobre o erro.
        /// </summary>
        public string Informação { get; } = null;

        #endregion

        /// <summary>
        /// Retorna um texto contendo as informações do token armazenado.
        /// </summary>
        public override string ToString()
        {
            return $"Erro semântico: '{this.Lexema}', Linha: {this.Linha}, Posição: {this.Posição}{(this.Informação == null ? "" : $". {this.Informação}")}";
        }
    }
}
