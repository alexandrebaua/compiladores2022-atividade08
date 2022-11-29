using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.GeradorCódigo.MIPS;

namespace Compilador.Semantico.Tipos.Auxiliar
{
    /// <summary>
    /// Classe para armazenar a informação de um comando de retorno de dados de uma função.
    /// </summary>
    public class FunçãoRetorne : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public FunçãoRetorne(List<SemanticoToken> elementos)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 3)
                throw new Exception("Um tipo 'função retorne' deve receber no mínimo 3 elementos!");

            this.Elementos = elementos.ToArray();
            
            List<SemanticoToken> elem = new List<SemanticoToken>();
            for (int i = 0; i < this.Elementos.Length - 2; i++)
                elem.Add(this.Elementos[i + 1]);

            this.Retorne = new Expressão(elem);
        }

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }
        
        /// <summary>
        /// Obtém ou define a variável pai (função) que está associada à este retorno.
        /// </summary>
        public Variável Pai { get; set; } = null;
        
        /// <summary>
        /// Obtém a expressão de retorno.
        /// </summary>
        public Expressão Retorne { get; }

        #region Validação Semântica

        /// <summary>
        /// Executa a validação semântica de uma expressão (aritmética, lógica ou texto) do retorno de função.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarVariáveis(List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            // Executa a validação semântica da expressão.
            this.Retorne.ValidarExpressão(this.Pai.Tipo, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            this.Retorne.GerarMips(mips, this.Pai.Tipo);
            
            switch (this.Pai.Tipo)
            {
                case VariávelTipo.Lógico:
                    // Este comando é para mover o resultado lógico fornecido do registrador $t4 para o registrador $s0
                    mips.SectionText.Adicionar(new MipsText("move", "$s0,$t4"));
                    break;

                case VariávelTipo.Inteiro:
                    // Este comando é para mover o resultado inteiro fornecido do registrador $t0 para o registrador $s0
                    mips.SectionText.Adicionar(new MipsText("move", "$s0,$t0"));
                    break;

                case VariávelTipo.Real:
                    // Este comando é para mover o resultado real fornecido do coprocessador 1 $f4 para o registrador $f20
                    mips.SectionText.Adicionar(new MipsText("mov.s", "$f20,$f4"));
                    break;

                case VariávelTipo.Caracter:
                    // Este comando é para mover o endereço da variável de texto do registrador $t0 para o registrador $s0
                    mips.SectionText.Adicionar(new MipsText("move", "$s0,$a0"));
                    break;
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
            foreach (var item in this.Retorne.Elementos)
                tmp += item.Lexema;

            return $"Função Retorne: '{tmp}'";
        }
    }
}
