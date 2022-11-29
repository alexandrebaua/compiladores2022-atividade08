using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.GeradorCódigo.MIPS;
using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Declaração de Variáveis'.
    /// </summary>
    public class DeclaraçãoSimples : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public DeclaraçãoSimples(List<SemanticoToken> elementos)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 4)
                throw new Exception("Um tipo 'declaração simples' deve receber no mínimo 4 elementos!");

            this.Elementos = elementos.ToArray();
            
            IEnumerable<SemanticoToken> faixas = this.Elementos.Where(x => x.Token.Equals("ID"));
            this.Identificadores = new SemanticoToken[faixas.Count()];
            for (int i = 0; i < this.Identificadores.Length; i++)
                this.Identificadores[i] = faixas.ElementAt(i);

            this.TipoDeDados = this.Elementos[this.Elementos.Length - 2];
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém o token do tipo de dados da declaração do vetor.
        /// </summary>
        public SemanticoToken TipoDeDados { get; }

        /// <summary>
        /// Obtém os tokens dos identificadores (nomes) da declaração.
        /// </summary>
        public SemanticoToken[] Identificadores { get; }

        #endregion

        /// <summary>
        /// Obtém as variáveis referêntes à esta declaração.
        /// </summary>
        public Variável[] ObterVariáveis()
        {
            List<Variável> list = new List<Variável>();
            foreach (var item in this.Identificadores)
                list.Add(new Variável(item, Variável.ConverterTipo(this.TipoDeDados), VariávelDeclaração.Simples));

            return list.ToArray();
        }

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            foreach (var id in this.Identificadores)
            {
                if (!id.Variável.Utilizado)
                    continue;

                switch (id.Variável.Tipo)
                {
                    case VariávelTipo.Lógico:
                        mips.HandlerLogico.AdicionarVariável(id.ObterEtiqueta());
                        break;

                    case VariávelTipo.Inteiro:
                        mips.SectionData.Adicionar(new MipsDataWord(id.ObterEtiqueta()));
                        break;

                    case VariávelTipo.Real:
                        mips.SectionData.Adicionar(new MipsDataFloat(id.ObterEtiqueta()));
                        break;

                    case VariávelTipo.Caracter:
                        mips.SectionData.Adicionar(new MipsDataSpace(id.ObterEtiqueta(), 34));

                        // Informa utilização de opcionais:
                        mips.HandlerOpcionais.VariáveisTextoUtilizado = true;
                        break;
                }
            }
        }

        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar o nó do comando.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó do Comando
            treeNode.Nodes.Add(this.ToString());
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty;
            for (int i = 0; i < this.Identificadores.Length; i++)
            {
                tmp += this.Identificadores[i].Lexema;
                if (i < this.Identificadores.Length - 1)
                    tmp += ", ";
            }
            
            return $"Declaração: '{tmp} : {this.TipoDeDados.Lexema}'";
        }
    }
}
