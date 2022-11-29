using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.GeradorCódigo.MIPS;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Interface para os tipos de comandos disponiveis na linguagem.
    /// </summary>
    public interface ITipo
    {
        /// <summary>
        /// Geração dos comandos Assembly MIPS.
        /// </summary>
        /// <param name="mips"></param>
        void GerarMips(MipsClass mips);

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode"></param>
        void GerarArvore(TreeNode treeNode);
    }
}
