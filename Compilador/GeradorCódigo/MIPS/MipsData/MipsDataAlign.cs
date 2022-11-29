using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS.MipsData
{
    /// <summary>
    /// Classe da diretiva de alinhamento de bordas de dados.
    /// Alinha a próxima referência em um limite de 2^n bytes.
    /// </summary>
    public class MipsDataAlign : IMipsData
    {
        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="bytes"></param>
        public MipsDataAlign(byte bytes)
        {
            this.Bytes = bytes;
        }

        /// <summary>
        /// Obtém a etiqueta para criar a referência da variável na memória.
        /// </summary>
        public string Etiqueta { get; } = null;

        /// <summary>
        /// Obtêm o valor de alinhamento n (2^n bytes).
        /// </summary>
        public byte Bytes { get; }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            return $".align {this.Bytes}";
        }
    }
}
