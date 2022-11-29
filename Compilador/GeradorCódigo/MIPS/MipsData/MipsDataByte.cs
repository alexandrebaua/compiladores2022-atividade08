using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS.MipsData
{
    /// <summary>
    /// Classe da diretiva de armazenamento de valores de 8-bit em sucessivas palavras de memória.
    /// </summary>
    public class MipsDataByte : IMipsData
    {
        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="valores">Valores a serem armazenados.</param>
        public MipsDataByte(string etiqueta, sbyte[] valores)
        {
            this.Etiqueta = etiqueta;
            this.Valores = valores;
        }

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="tamanho">Quantidade de valores 8-bit inicializados em 0 (zero) a serem armazenados.</param>
        public MipsDataByte(string etiqueta, int tamanho = 1)
        {
            this.Etiqueta = etiqueta;
            this.Valores = new sbyte[tamanho];
        }

        /// <summary>
        /// Obtém a etiqueta para criar a referência da variável na memória.
        /// </summary>
        public string Etiqueta { get; }

        /// <summary>
        /// Obtém os valores armazenados.
        /// </summary>
        public sbyte[] Valores { get; }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty;
            for (int i = 0; i < this.Valores.Length; i++)
            {
                tmp += $"0x{this.Valores[i]:X2}";
                if (i < this.Valores.Length - 1)
                    tmp += ", ";
            }
            return $"{this.Etiqueta}: .byte {tmp}";
        }
    }
}
