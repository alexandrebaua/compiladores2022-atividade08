using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS.MipsData
{
    /// <summary>
    /// Classe da diretiva de armazenamento de texto ASCII na memória e a termina em nulo (0).
    /// </summary>
    public class MipsDataAsciiz : IMipsData
    {
        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="texto">O texto ASCII para armazenar.</param>
        public MipsDataAsciiz(string etiqueta, string texto)
        {
            this.Etiqueta = etiqueta;

            if (texto.StartsWith("\"") && texto.StartsWith("\""))
                this.Texto = texto;
            else
                this.Texto = $"\"{texto}\"";
        }

        /// <summary>
        /// Obtém a etiqueta para criar a referência da variável na memória.
        /// </summary>
        public string Etiqueta { get; }

        /// <summary>
        /// Obtém o texto ASCII armazenado.
        /// </summary>
        public string Texto { get; }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            return $"{this.Etiqueta}: .asciiz {this.Texto}";
        }
    }
}
