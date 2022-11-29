using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Exceptions;
using Compilador.GeradorCódigo.MIPS;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Função Principal'.
    /// </summary>
    public class Principal : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        /// <param name="listaTiposEscopoMaior">Elementos do tipo comando em escopos seguintes à este comando.</param>
        public Principal(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
            this.Elementos = elementos.ToArray();
            this.ListaTiposEscopoMaior = listaTiposEscopoMaior.ToArray();

            List<ITipo> variáveis = new List<ITipo>();
            List<ITipo> comandos = new List<ITipo>();
            foreach (var item in this.ListaTiposEscopoMaior)
            {
                if (item is DeclaraçãoSimples || item is DeclaraçãoVetor)
                    variáveis.Add(item);
                else
                    comandos.Add(item);
            }

            if (variáveis.Count > 0)
                this.Variáveis = variáveis.ToArray();

            this.Comandos = comandos.ToArray();
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
        /// Obtém as variáveis declaradas no escopo da função principal.
        /// </summary>
        public ITipo[] Variáveis { get; } = null;

        /// <summary>
        /// Obtém os comandos que compõe a função principal.
        /// </summary>
        public ITipo[] Comandos { get; }

        #endregion

        #region Validação Semântica

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarVariáveis(List<Variável> listaVariáveisGlobal, List<SemanticoError> listaErrors, ListBox debug)
        {
            base.VerificarVariáveis(listaVariáveisGlobal, null, this.Variáveis, this.Comandos, listaErrors, debug);
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            // Se existirem variáveis locais, então:
            if (this.Variáveis != null)
            {
                // Adiciona as variáveis:
                foreach (var item in this.Variáveis)
                    item.GerarMips(mips);
            }

            mips.SectionText.Adicionar(new MipsText("#", "Inicio da função principal"));
            mips.SectionText.Adicionar(new MipsText("main:", null));
            mips.SectionText.Adicionar(MipsText.Blank);

            // Adiciona as instruções dos comandos:
            foreach (var comando in this.Comandos)
                comando.GerarMips(mips);
        }

        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar os nós subsequentes.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó da Função Principal
            TreeNode node = treeNode.Nodes.Add(this.ToString());

            if (this.Variáveis != null)
            {
                TreeNode varLocal = node.Nodes.Add("Variáveis Locais");
                foreach (var item in this.Variáveis)
                    varLocal.Nodes.Add(item.ToString());
            }

            if (this.Comandos != null)
            {
                foreach (var comando in this.Comandos)
                    comando.GerarArvore(node);
            }
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            int var = 0, dec = 0;
            if (this.Variáveis != null)
            {
                foreach (var item in this.Variáveis)
                {
                    if (item is DeclaraçãoVetor)
                        var += 1;
                    else if (item is DeclaraçãoSimples)
                        var += ((DeclaraçãoSimples)item).Identificadores.Length;
                }

                dec = this.Variáveis.Count();
            }

            return $"Principal: {var} variável local ({dec} declaração), {(this.Comandos == null ? 0 : this.Comandos.Count())} comandos";
        }
    }
}
