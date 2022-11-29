using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS
{
    /// <summary>
    /// Classe de uma instrução para o segmento de texto no código Assembly MIPS.
    /// </summary>
    public class MipsText
    {
        /// <summary>
        /// Define uma linha vazia o código Assembly MIPS.
        /// </summary>
        public static readonly MipsText Blank = new MipsText(null, null);

        /// <summary>
        /// Define o instrução de chamada de sistema do Assembly MIPS.
        /// </summary>
        public static readonly MipsText Syscall = new MipsText("syscall", null);

        /// <summary>
        /// Define a instrução de finalização da execução do sistema.
        /// </summary>
        public static readonly MipsText Exit = new MipsText("li", "$v0,10");

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="instrução"></param>
        /// <param name="argumentos"></param>
        public MipsText(string instrução, string argumentos)
        {
            this.Instrução = instrução;
            this.Argumentos = argumentos;
        }

        /// <summary>
        /// Obtém a instrução armazenada.
        /// </summary>
        public string Instrução { get; }

        /// <summary>
        /// Obtém ou define os argumentos da instrução armazenada.
        /// </summary>
        public string Argumentos { get; set; }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            if (this.Instrução == null)
                return String.Empty;

            if (this.Argumentos == null)
                return this.Instrução;

            return $"{this.Instrução} {this.Argumentos}";
        }
    }
}
