using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Semantico.Tipos.Auxiliar
{
    /// <summary>
    /// Classe para declaração dos parâmetros de entrada das funções ou procedimentos.
    /// </summary>
    public class ParâmetroProcFunc
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public ParâmetroProcFunc(List<SemanticoToken> elementos)
        {
            this.Elementos = elementos.ToArray();

            int i = 0;
            if (elementos.First().Token.Equals("VAR"))
            {
                this.Ponteiro = true;
                i = 1;
            }

            List<SemanticoToken> ids = new List<SemanticoToken>();
            while (i < this.Elementos.Length - 2)
            {
                if (this.Elementos[i].Token.Equals("VIRGULA"))
                {
                    i++;
                    continue;
                }

                ids.Add(this.Elementos[i++]);
            }

            this.Identificadores = ids.ToArray();

            this.TipoDeDados = this.Elementos.Last();
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém o token que possui a informação do tipo de dados do parâmetro declarado.
        /// </summary>
        public SemanticoToken TipoDeDados { get; }

        /// <summary>
        /// Obtém o(s) identificador(es) d(os) parâmetro(s) declarados na função/procedimento.
        /// </summary>
        public SemanticoToken[] Identificadores { get; }

        /// <summary>
        /// Obtém a informação se o parâmetro é por passagem de ponteiro.
        /// </summary>
        public bool Ponteiro { get; } = false;

        #endregion

        /// <summary>
        /// Obtém uma ou mais variáveis referênte à esta declaração.
        /// </summary>
        public Variável[] ObterVariáveis()
        {
            List<Variável> list = new List<Variável>();
            foreach (var item in this.Identificadores)
                list.Add(new Variável(item, Variável.ConverterTipo(this.TipoDeDados), VariávelDeclaração.Parametro, true, this.Ponteiro));

            return list.ToArray();
        }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = this.Ponteiro ? "var " : String.Empty;
            for (int i = 0; i < this.Identificadores.Length; i++)
            {
                tmp += this.Identificadores[i].Lexema;
                if (i < this.Identificadores.Length - 1)
                    tmp += ", ";
            }

            return $"{tmp}: {this.TipoDeDados.Lexema}";
        }
    }
}
