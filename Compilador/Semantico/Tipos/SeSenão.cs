using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.GeradorCódigo.MIPS;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Se-Senão'.
    /// </summary>
    public class SeSenão : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        /// <param name="listaTiposEscopoMaior">Elementos do tipo comando em escopos seguintes à este comando.</param>
        public SeSenão(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 5)
                throw new Exception("Um tipo 'se-senão' deve receber no mínimo 5 elementos!");

            this.Elementos = elementos.ToArray();
            this.ListaTiposEscopoMaior = listaTiposEscopoMaior.ToArray();

            List<SemanticoToken> elem = new List<SemanticoToken>();
            for (int i = 1; i < this.Elementos.Length - 3; i++)
            {
                if (this.Elementos[i].Token.Equals("ENTAO"))
                    break;

                elem.Add(this.Elementos[i]);
            }
            this.ExpressãoLógica = new Expressão(elem);

            List<ITipo> bloco = new List<ITipo>();
            foreach (var item in this.ListaTiposEscopoMaior)
            {
                if (item is Senão)
                {
                    this.BlocoA = bloco.ToArray();
                    bloco = new List<ITipo>();
                    continue;
                }

                bloco.Add(item);
            }

            if (bloco.Count() > 0)
            {
                if(this.BlocoA == null)  // Se não foi encontrado um comando 'senão', então os comandos são do bloco A:
                    this.BlocoA = bloco.ToArray();
                else  // Senão, foi encontrado um comando 'senão', e o bloco A recebeu a lista de comandos, então agora os comandos são do bloco B:
                    this.BlocoB = bloco.ToArray();
            }
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém os elementos do tipo comando em escopos seguintes à este comando.
        /// </summary>
        public ITipo[] ListaTiposEscopoMaior { get; }

        /// <summary>
        /// Obtém a expressão lógica condicional do comando de seleção para a condição 'se'.
        /// </summary>
        public Expressão ExpressãoLógica { get; }

        /// <summary>
        /// Obtém os elementos do tipo comando que compõe o Bloco A do comando se seleção.
        /// </summary>
        public ITipo[] BlocoA { get; }

        /// <summary>
        /// Obtém os elementos do tipo comando que compõe o Bloco B do comando se seleção.
        /// </summary>
        public ITipo[] BlocoB { get; } = null;

        #endregion

        #region Validação Semântica

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarVariáveis(List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            this.ExpressãoLógica.ValidarExpressãoLógica(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
            
            base.VerificarVariáveisComandos(listaVariáveisGlobal, listaVariáveisLocal, this.BlocoA, listaErrors, debug);

            if (this.BlocoB != null)
                base.VerificarVariáveisComandos(listaVariáveisGlobal, listaVariáveisLocal, this.BlocoB, listaErrors, debug);
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            // Gera as instruções da expressão lógica condicional, e efetua o teste do resultado lógico:
            this.ExpressãoLógica.GerarMips(mips, VariávelTipo.Lógico);
            mips.SectionText.Adicionar(new MipsText("beq", $"$t4,1,seBlocoA_{this.Elementos[0].Index}"));

            // Se o bloco B possuir comandos, então:
            if (this.BlocoB != null)
            {
                // Adiciona os comandos do bloco B:
                foreach (var comando in this.BlocoB)
                    comando.GerarMips(mips);
            }
            mips.SectionText.Adicionar(new MipsText("j", $"seFim_{this.Elementos[0].Index}"));
            mips.SectionText.Adicionar(new MipsText($"seBlocoA_{this.Elementos[0].Index}:", null));

            // Adiciona os comandos do bloco A:
            foreach (var comando in this.BlocoA)
                comando.GerarMips(mips);

            mips.SectionText.Adicionar(new MipsText($"seFim_{this.Elementos[0].Index}:", null));
            mips.SectionText.Adicionar(MipsText.Blank);
        }

        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar os nós subsequentes.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó do Comando
            TreeNode node = treeNode.Nodes.Add(this.ToString());

            TreeNode nodeBlocoA = node.Nodes.Add("Bloco A");
            foreach (var comando in this.BlocoA)
                comando.GerarArvore(nodeBlocoA);

            if (this.BlocoB != null)
            {
                node.Nodes.Add("Senão");
                TreeNode nodeBlocoB = node.Nodes.Add("Bloco B");
                foreach (var comando in this.BlocoB)
                    comando.GerarArvore(nodeBlocoB);
            }
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string argLogico = String.Empty;
            foreach (var item in this.ExpressãoLógica.Elementos)
                argLogico += $"{item.Lexema} ";

            return $"Se-Senão: '{argLogico}', Bloco A: {(this.BlocoA == null ? 0 : this.BlocoA.Count())} comandos, Bloco B: {(this.BlocoB == null ? 0 : this.BlocoB.Count())} comandos";
        }
    }
}
