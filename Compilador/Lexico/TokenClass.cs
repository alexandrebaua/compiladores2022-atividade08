using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Sintatico.Ascendente.SLR;

namespace Compilador.Lexico
{
    /// <summary>
    /// Classe de armazenamento das informações de um token.
    /// </summary>
    public class TokenClass
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="lexema">O lexema à armazenar.</param>
        /// <param name="token">O token à armazenar.</param>
        /// <param name="linha">Linha onde foi encontrado.</param>
        /// <param name="posicaoFinal">Posição final do lexema na linha.</param>
        /// <param name="index">Posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.</param>
        public TokenClass(string lexema, string token, int linha, int posicaoFinal, int index) : this(lexema, token)
        {
            this.Linha = linha;
            this.Inicio = posicaoFinal - lexema.Length + 1;
            this.Final = posicaoFinal;
            //this.Index = index - lexema.Length + 1;
            this.Index = index - lexema.Length;
        }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="lexema">O lexema à armazenar.</param>
        /// <param name="token">O token à armazenar.</param>
        public TokenClass(string lexema, string token)
        {
            this.Lexema = lexema;
            this.Token = token;
        }

        /// <summary>
        /// Construtor da classe. Necessário para a classe <see cref="Semantico.SemanticoToken"/>.
        /// </summary>
        /// <param name="token">O token à armazenar.</param>
        protected TokenClass(TokenClass token)
        {
            this.Lexema = token.Lexema;
            this.Token = token.Token;
            this.Linha = token.Linha;
            this.Inicio = token.Inicio;
            this.Final = token.Final;
            this.Index = token.Index;
        }

        /// <summary>
        /// Obtém ou define o lexema.
        /// </summary>
        public string Lexema { get; set; }

        /// <summary>
        /// Obtém ou define o token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Obtém ou define o número da linha onde foi encontrado o lexema.
        /// </summary>
        public int Linha { get; set; } = 0;

        /// <summary>
        /// Obtém ou define a posição inicial onde foi encontrado o lexema na linha.
        /// </summary>
        public int Inicio { get; set; } = 0;

        /// <summary>
        /// Obtém ou define a posição final onde foi encontrado o lexema na linha.
        /// </summary>
        public int Final { get; set; } = 0;

        /// <summary>
        /// A posição inicial absoluta baseada em zero do primeiro caracter encontrado para o lexema no texto.
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// Retorna um texto contendo as informações do token armazenado.
        /// </summary>
        public override string ToString()
        {
            if (this.Linha != 0)
            {
                if (this.Inicio == this.Final)
                    return $"Linha: {this.Linha}, Posição: {this.Inicio}, Index: {this.Index}, Token: {this.Token}, Lexema: '{this.Lexema}'";
                else
                    return $"Linha: {this.Linha}, Posição: {this.Inicio}..{this.Final}, Index: {this.Index}, Token: {this.Token}, Lexema: '{this.Lexema}'";
            }

            return $"Token: {this.Token}, Lexema: '{this.Lexema}'";
        }
    }
}
