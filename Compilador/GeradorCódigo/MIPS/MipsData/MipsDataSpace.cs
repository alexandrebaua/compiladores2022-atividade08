using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS.MipsData
{
    /// <summary>
    /// Classe da diretiva de armazenamento de espaço vazio na memória.
    /// Deixa vazio uma região de n-byte na memória para uso posterior.
    /// </summary>
    public class MipsDataSpace : IMipsData
    {
        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="espaço"></param>
        public MipsDataSpace(string etiqueta, int espaço)
        {
            this.Etiqueta = etiqueta;
            this.Espaço = espaço;
        }

        /// <summary>
        /// Obtém a etiqueta para criar a referência da variável na memória.
        /// </summary>
        public string Etiqueta { get; }

        /// <summary>
        /// Obtém o valor de n-byte de espaço a ser deixado vazio.
        /// </summary>
        public int Espaço { get; }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            return $"{this.Etiqueta}: .space {this.Espaço}";
        }
    }
}
