using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Lexico;
using Compilador.Semantico.Tipos.Auxiliar;
using Compilador.Sintatico.Ascendente.SLR;

namespace Compilador.Semantico
{
    /// <summary>
    /// Classe de armazenamento das informações de um token, com informações extendidas para análise semântica.
    /// </summary>
    public class SemanticoToken : TokenClass
    {
        /// <summary>
        /// Construtor da classe. Cria uma cópia de um token.
        /// </summary>
        /// <param name="token">O token à armazenar.</param>
        public SemanticoToken(TokenClass token) : base(token) { }

        /// <summary>
        /// Obtém ou define o nível de escopo que o token pertence (será definido durante a análise sintática).
        /// </summary>
        public int Escopo { get; set; } = -1;

        /// <summary>
        /// Obtém ou define o agrupamento que o token pertence (será definido durante a análise sintática).
        /// </summary>
        public int Grupo { get; set; } = -1;

        /// <summary>
        /// Obtém ou define o tipo de redução encontrado durante a análise sintática.
        /// </summary>
        public ReduceTypeEnum ReduceType { get; set; } = ReduceTypeEnum.None;

        /// <summary>
        /// Obtém ou define se o tipo de redução encontrado durante a análise sintática é final ou intermediária.
        /// </summary>
        public bool ReduçãoFinal { get; set; } = false;

        /// <summary>
        /// Obtém ou define uma variável associada ao token.
        /// </summary>
        public Variável Variável { get; set; } = null;

        /// <summary>
        /// Executa a união de uma constante com seu sinal negativo.
        /// </summary>
        /// <param name="semToken">O token contendo o sinal negativo.</param>
        public void Unir(SemanticoToken semToken)
        {
            if (!semToken.Token.Equals("MENOS") || (!this.Token.Equals("CONST_INT") && !this.Token.Equals("CONST_REAL")))
                throw new Exception("A união de tokens é permitida apenas entre o sinal de negativo e uma constante numérica!");

            this.Inicio = semToken.Inicio;
            this.Index = semToken.Index;
            base.Lexema = $"-{base.Lexema}";
        }

        /// <summary>
        /// Obtém uma etiqueta padronizada para identificação de variáveis.
        /// </summary>
        /// <returns>Uma etiqueta para a variável.</returns>
        public string ObterEtiqueta()
        {
            string label = String.Empty;

            if (this.Variável == null)
            {
                switch (this.Token)
                {
                    case "CONST_LOGICA":
                        label = "Log";
                        break;

                    case "CONST_INT":
                        label = "Int";
                        break;

                    case "CONST_REAL":
                        label = "Real";
                        break;

                    case "CONST_TEXTO":
                        label = "Car";
                        break;

                    default:
                        throw new Exception($"Criação de etiqueta não suportada para o token '{this.Token}'.");
                }

                return $"const{label}_{this.Index}";
            }

            switch (this.Variável.TipoDeclaração)
            {
                case VariávelDeclaração.Desconhecido:
                    break;
                case VariávelDeclaração.Simples:
                    label = "var";
                    break;
                case VariávelDeclaração.Vetor:
                    label = "vet";
                    break;
                case VariávelDeclaração.Procedimento:
                    break;
                case VariávelDeclaração.Função:
                    break;
                case VariávelDeclaração.Parametro:
                    label = "pr";
                    break;
                case VariávelDeclaração.FunçãoRetorno:
                    break;
                case VariávelDeclaração.Agrupamento:
                    label = "tmp";
                    break;
                default:
                    break;
            }

            switch (this.Variável.Tipo)
            {
                case VariávelTipo.Desconhecido:
                    break;
                case VariávelTipo.Lógico:
                    label += "Log";
                    break;
                case VariávelTipo.Inteiro:
                    label += "Int";
                    break;
                case VariávelTipo.Real:
                    label += "Real";
                    break;
                case VariávelTipo.Caracter:
                    label += "Car";
                    break;
                default:
                    break;
            }

            if (this.Variável.TipoDeclaração == VariávelDeclaração.Agrupamento)
            {
                if (this.Variável.Global)
                    return label;

                return $"{label}_{this.Variável.Token.Index}";
            }

            if (this.Variável.Ponteiro)
                label += "_ptr";

            if (this.Variável.Global)
                return $"{label}_{this.Lexema}";

            return $"{label}_{this.Variável.Token.Index}_{this.Lexema}";
        }

        /// <summary>
        /// Retorna um texto contendo as informações do token armazenado.
        /// </summary>
        public override string ToString()
        {
            if (this.Linha != 0)
            {
                if (this.Escopo < 0)
                {
                    if (this.Inicio == this.Final)
                        return $"Linha: {this.Linha}, Posição: {this.Inicio}, Index: {this.Index}, Token: {this.Token}, Lexema: '{this.Lexema}'";
                    else
                        return $"Linha: {this.Linha}, Posição: {this.Inicio}..{this.Final}, Index: {this.Index}, Token: {this.Token}, Lexema: '{this.Lexema}'";
                }

                if (this.ReduceType != ReduceTypeEnum.None)
                    return $"({this.ReduceType}) Escopo: {this.Escopo}, Grupo: {this.Grupo}, Linha: {this.Linha}, Token: {this.Token}, Lexema: '{this.Lexema}'";

                return $"Escopo: {this.Escopo}, Grupo: {this.Grupo}, Linha: {this.Linha}, Token: {this.Token}, Lexema: '{this.Lexema}'";
            }

            return $"Token: {this.Token}, Lexema: '{this.Lexema}'";
        }
    }
}
