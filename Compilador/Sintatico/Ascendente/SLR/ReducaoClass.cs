using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Sintatico.Ascendente.SLR
{
    /// <summary>
    /// Enumerador contendo os tipos de reduções executadas pelo analisador sintático.
    /// </summary>
    public enum ReduceTypeEnum : byte
    {
        /// <summary>
        /// Sem tipo de redução.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Redução principal do programa.
        /// </summary>
        Algoritmo = 0x01,

        /// <summary>
        /// Redução da função principal.
        /// </summary>
        Principal = 0x02,

        /// <summary>
        /// Redução para procedimentos.
        /// </summary>
        Procedimento = 0x03,

        /// <summary>
        /// Redução para funções.
        /// </summary>
        Função = 0x04,

        /// <summary>
        /// Redução da operação de seleção condicional If.
        /// </summary>
        SeleçãoIf = 0x05,

        /// <summary>
        /// Redução da operação do laço de repeticção 'Enquanto'.
        /// </summary>
        Enquanto = 0x06,

        /// <summary>
        /// Redução da operação do laço de repeticção 'Repita'.
        /// </summary>
        Repita = 0x07,

        /// <summary>
        /// Redução da operação do laço de repetição 'Enquanto'.
        /// </summary>
        ParaFaça = 0x08,

        /// <summary>
        /// Redução da declaração de variáveis.
        /// </summary>
        Declaração = 0x40,

        /// <summary>
        /// Redução da operação de atribuição.
        /// </summary>
        AtribuiçãoNumérica = 0x41,

        /// <summary>
        /// Redução da operação de atribuição.
        /// </summary>
        AtribuiçãoLogica = 0x42,

        /// <summary>
        /// Redução da operação de leitura de entrada do teclado.
        /// </summary>
        Leia = 0x43,

        /// <summary>
        /// Redução da operação de escrita.
        /// </summary>
        Escreva = 0x44,

        /// <summary>
        /// Redução da operação de escrita com quebra de linha.
        /// </summary>
        Escrevaln = 0x45,

        /// <summary>
        /// Redução da operação da chamada de procedimento.
        /// </summary>
        ChamaProcedimento = 0x46,

        /// <summary>
        /// Redução para retorno das funções.
        /// </summary>
        FunçãoRetorne = 0x47,

        /// <summary>
        /// Redução da operação de declaração de variáveis dos tipos simples.
        /// </summary>
        DeclaraçãoSimples = 0x80,

        /// <summary>
        /// Redução da operação de declaração de variáveis do tipo vetor.
        /// </summary>
        DeclaraçãoVetor = 0x81
    }

    /// <summary>
    /// Classe de armazenamento de uma redução do analisador sintático SLR.
    /// </summary>
    public class ReducaoClass
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="id">Identificação da redução.</param>
        /// <param name="to">Simbolo não terminal da redução.</param>
        /// <param name="from">Vetor com os simbolos a serem reduzidos (desempilhados).</param>
        /// <param name="reduceType">O tipo de redução encontrado durante a analise sintática.</param>
        public ReducaoClass(int id, string to, string[] from, ReduceTypeEnum reduceType = ReduceTypeEnum.None)
        {
            this.ID = id;
            this.To = to;
            this.From = from;
            this.ReduceType = reduceType;
        }

        /// <summary>
        /// Obtém ou define o código de identificação da redução.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Obtém ou define o simbolo não terminal da redução.
        /// </summary>
        public string[] From { get; set; }

        /// <summary>
        /// Obtém ou define o vetor com os simbolos a serem reduzidos (desempilhados).
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de redução encontrado durante a analise sintática.
        /// </summary>
        public ReduceTypeEnum ReduceType { get; set; }

        /// <summary>
        /// Retorna um texto contendo as informações do token armazenado.
        /// </summary>
        public override string ToString()
        {
            string from = String.Empty;
            foreach (var item in this.From)
                from += $"{item} ";

            if (String.IsNullOrEmpty(from))
                from = "ε";

            return $"r{this.ID} = {this.To} --> {from.Trim()}";
        }
    }
}
