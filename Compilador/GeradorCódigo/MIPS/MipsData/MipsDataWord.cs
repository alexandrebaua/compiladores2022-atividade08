using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS.MipsData
{
    /// <summary>
    /// Classe da diretiva de armazenamento de valores de 32-bit em sucessivas palavras de memória.
    /// </summary>
    public class MipsDataWord : IMipsData
    {
        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="valores">Valores a serem armazenados.</param>
        public MipsDataWord(string etiqueta, int[] valores)
        {
            this.Etiqueta = etiqueta;
            this.Valores = valores;
        }

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="tamanho">Quantidade de valores 32-bit inicializados em 0 (zero) a serem armazenados.</param>
        public MipsDataWord(string etiqueta, int tamanho = 1)
        {
            this.Etiqueta = etiqueta;
            this.Valores = new int[tamanho];
        }

        /// <summary>
        /// Obtém a etiqueta para criar a referência da variável na memória.
        /// </summary>
        public string Etiqueta { get; }

        /// <summary>
        /// Obtém os valores armazenados.
        /// </summary>
        public int[] Valores { get; }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty;
            for (int i = 0; i < this.Valores.Length; i++)
            {
                tmp += this.Valores[i].ToString();
                if (i < this.Valores.Length - 1)
                    tmp += ", ";
            }
            return $"{this.Etiqueta}: .word {tmp}";
        }
    }
}
