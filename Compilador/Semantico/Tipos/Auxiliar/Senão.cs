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
    /// Classe do comando senão, que faz parte do comando condicional 'se-senão'.
    /// </summary>
    public class Senão : ITipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="token">O token do comando 'senão'.</param>
        public Senão(SemanticoToken token)
        {
            this.Token = token;
        }

        /// <summary>
        /// Obtém o token do comando 'senão', que faz parte do comando condicional 'se-senão'.
        /// </summary>
        public SemanticoToken Token { get; }

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// Atenção: não implementado!
        /// </summary>
        /// <param name="mips"></param>
        public void GerarMips(MipsClass mips)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// Atenção: não implementado!
        /// </summary>
        /// <param name="treeNode"></param>
        public void GerarArvore(TreeNode treeNode)
        {
            throw new NotImplementedException();
        }
    }
}
