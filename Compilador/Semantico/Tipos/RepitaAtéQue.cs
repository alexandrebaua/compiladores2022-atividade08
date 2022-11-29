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
    /// Classe para o comando do tipo 'Repita-Até-Que'.
    /// </summary>
    public class RepitaAtéQue : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        /// <param name="listaTiposEscopoMaior">Elementos do tipo comando em escopos seguintes à este comando.</param>
        public RepitaAtéQue(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 5)
                throw new Exception("Um tipo 'repita-até-que' deve receber no mínimo 5 elementos!");

            this.Elementos = elementos.ToArray();
            this.ListaTiposEscopoMaior = listaTiposEscopoMaior.ToArray();

            List<SemanticoToken> elem = new List<SemanticoToken>();
            for (int i = 3; i < this.Elementos.Length - 1; i++)
                elem.Add(this.Elementos[i]);

            this.ExpressãoLógica = new Expressão(elem);
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
        /// Obtém a expressão lógica condicional do comando de repetição.
        /// </summary>
        public Expressão ExpressãoLógica { get; }

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

            base.VerificarVariáveisComandos(listaVariáveisGlobal, listaVariáveisLocal, this.ListaTiposEscopoMaior, listaErrors, debug);
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            mips.SectionText.Adicionar(new MipsText($"repitaIni_{this.Elementos[0].Index}:", null));

            foreach (var comando in this.ListaTiposEscopoMaior)
                comando.GerarMips(mips);

            this.ExpressãoLógica.GerarMips(mips, VariávelTipo.Lógico);
            mips.SectionText.Adicionar(new MipsText("beq", $"$t4,1,repitaFim_{this.Elementos[0].Index}"));

            mips.SectionText.Adicionar(new MipsText("j", $"repitaIni_{this.Elementos[0].Index}"));
            mips.SectionText.Adicionar(new MipsText($"repitaFim_{this.Elementos[0].Index}:", null));
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

            if (this.ListaTiposEscopoMaior != null)
            {
                foreach (var comando in this.ListaTiposEscopoMaior)
                    comando.GerarArvore(node);
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

            return $"Repita-Até-Que: '{argLogico}', {(this.ListaTiposEscopoMaior == null ? 0 : this.ListaTiposEscopoMaior.Count())} comandos";
        }
    }
}
