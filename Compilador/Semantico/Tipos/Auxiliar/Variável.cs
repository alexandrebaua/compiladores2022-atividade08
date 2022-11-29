using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Semantico.Tipos.Auxiliar
{
    /// <summary>
    /// Tipo da variável.
    /// Tipos de dados primitivos.
    /// </summary>
    public enum VariávelTipo : byte
    {
        /// <summary>
        /// Tipo da variável desconhecido ou não inicializado.
        /// </summary>
        Desconhecido = 0x00,

        /// <summary>
        /// Valores lógicos ou booleanos. Pode assumir apenas dois valores VERDADEIRO ou FALSO.
        /// </summary>
        Lógico = 0x01,

        /// <summary>
        /// Números inteiros (positivos ou negativos).
        /// </summary>
        Inteiro = 0x02,

        /// <summary>
        /// Números reais (positivos ou negativos).
        /// </summary>
        Real = 0x03,

        /// <summary>
        /// Valores de texto (múltiplos caracteres).
        /// </summary>
        Caracter = 0x04
    }

    /// <summary>
    /// Tipos de declaração das variáveis.
    /// </summary>
    public enum VariávelDeclaração : byte
    {
        /// <summary>
        /// Tipo da declaração desconhecido ou não inicializado.
        /// </summary>
        Desconhecido = 0x00,

        /// <summary>
        /// Variável única.
        /// </summary>
        Simples = 0x01,

        /// <summary>
        /// Sequência de variáveis de mesmo tipo e referenciadas por um nome único.
        /// </summary>
        Vetor = 0x02,

        /// <summary>
        /// Chamada de procedimento.
        /// </summary>
        Procedimento = 0x03,

        /// <summary>
        /// Chamada de função.
        /// </summary>
        Função = 0x04,

        /// <summary>
        /// Parâmetro de procedimento ou função.
        /// </summary>
        Parametro = 0x05,

        /// <summary>
        /// Retorno de função.
        /// </summary>
        FunçãoRetorno = 0x06,

        /// <summary>
        /// Agrupamento por parênteses.
        /// </summary>
        Agrupamento = 0x07
    }

    /// <summary>
    /// Classe para armazenamento das informações de uma variável.
    /// </summary>
    public class Variável
    {
        /// <summary>
        /// Define uma variável fícticia.
        /// </summary>
        public static readonly Variável Dummy = new Variável();

        #region Variáveis privadas
        private bool _valorAtribuido = false;
        #endregion

        #region Construtores

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        private Variável()
        {
            this.Token = null;
            this.Tipo = VariávelTipo.Desconhecido;
            this.TipoDeclaração = VariávelDeclaração.Desconhecido;
        }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="token">O token do identificador da declaração desta variável.</param>
        /// <param name="tipo">O tipo de dados da variável.</param>
        /// <param name="tipoDeclaração">O tipo de declaração da variável.</param>
        /// <param name="valorAtribuido">Indica se a variável possui valor atribuido.</param>
        /// <param name="ponteiro">Indica se a variável é por passagem de ponteiro.</param>
        public Variável(SemanticoToken token, VariávelTipo tipo, VariávelDeclaração tipoDeclaração, bool valorAtribuido = false, bool ponteiro = false)
        {
            this.Token = token;
            this.Tipo = tipo;
            this.TipoDeclaração = tipoDeclaração;
            this._valorAtribuido = valorAtribuido;
            this.Ponteiro = ponteiro;
            token.Variável = this;
        }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="decVetor">A declaração do vetor que será associada à esta variável.</param>
        public Variável(DeclaraçãoVetor decVetor)
        {
            this.Declaração = decVetor;
            this.Token = decVetor.Identificador;
            this.Tipo = Variável.ConverterTipo(decVetor.TipoDeDados);
            this.TipoDeclaração = VariávelDeclaração.Vetor;
            this._valorAtribuido = true;
            decVetor.Identificador.Variável = this;
        }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="decProc">A declaração do procedimento que será associada à esta variável.</param>
        public Variável(Procedimento decProc)
        {
            this.Declaração = decProc;
            this.Token = decProc.Nome;
            this.Tipo = VariávelTipo.Desconhecido;
            this.TipoDeclaração = VariávelDeclaração.Procedimento;
            decProc.Nome.Variável = this;
        }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="decFunc">A declaração da função que será associada à esta variável.</param>
        public Variável(Função decFunc)
        {
            this.Declaração = decFunc;
            this.Token = decFunc.Nome;
            this.Tipo = Variável.ConverterTipo(decFunc.TipoRetorno);
            this.TipoDeclaração = VariávelDeclaração.Função;
            this._valorAtribuido = true;
            decFunc.Retorne.Pai = this;
            decFunc.Nome.Variável = this;
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Obtém a declaração associada à esta variável (apenas vetores, funções e procedimentos).
        /// </summary>
        public Tipo Declaração { get; } = null;

        /// <summary>
        /// Obtém o token do identificador da declaração desta variável.
        /// </summary>
        public SemanticoToken Token { get; }

        /// <summary>
        /// Obtém o identificador da variável.
        /// </summary>
        public string Identificador { get { return this.Token.Lexema; } }

        /// <summary>
        /// Obtém ou define o tipo de dados da variável.
        /// </summary>
        public VariávelTipo Tipo { get; set; }

        /// <summary>
        /// Obtém o tipo de declaração da variável.
        /// </summary>
        public VariávelDeclaração TipoDeclaração { get; }

        /// <summary>
        /// Obtém ou define a informação se a variável possui valor atribuido.
        /// </summary>
        public bool ValorAtribuido
        {
            get { return this._valorAtribuido; }

            set
            {
                this._valorAtribuido = value;
                this.Utilizado = true;
            }
        }

        /// <summary>
        /// Obtém ou define a informação se a variável foi utilizada.
        /// </summary>
        public bool Utilizado { get; set; } = false;

        /// <summary>
        /// Obtém ou define a informação se a variável é de escopo global ou local de um procedimento ou função.
        /// </summary>
        public bool Global { get; set; } = false;

        /// <summary>
        /// Obtém a informação se a variável é por passagem de ponteiro.
        /// Utilizado apenas para os parâmetros das funções ou procedimentos.
        /// </summary>
        public bool Ponteiro { get; } = false;

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            return $"Variável: '{this.Identificador}' : {this.Tipo}{(this.ValorAtribuido ? " : >Atribuido<" : "")}{(this.Utilizado ? " : >Utilizado<" : "")}";
        }

        /// <summary>
        /// Converte um token contendo a informação do tipo da variável, para um enumerador de tipos de variável.
        /// </summary>
        /// <param name="token">O token do tipo da declaração.</param>
        /// <returns>O enumerador do tipo da variável.</returns>
        public static VariávelTipo ConverterTipo(SemanticoToken token)
        {
            switch (token.Token)
            {
                case "LOGICO":
                    return VariávelTipo.Lógico;

                case "INTEIRO":
                    return VariávelTipo.Inteiro;

                case "REAL":
                    return VariávelTipo.Real;

                case "CARACTER":
                    return VariávelTipo.Caracter;
            }

            return VariávelTipo.Desconhecido;
        }
    }
}
