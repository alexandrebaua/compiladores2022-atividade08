using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS.MipsData
{
    /// <summary>
    /// Classe da diretiva de armazenamento de valores de ponto flutuante com precisão simples de 32-bit em sucessivas palavras de memória.
    /// </summary>
    public class MipsDataFloat : IMipsData
    {
        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="valores">Valores a serem armazenados.</param>
        public MipsDataFloat(string etiqueta, float[] valores)
        {
            this.Etiqueta = etiqueta;
            this.Valores = valores;
        }

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="etiqueta">A etiqueta para criar a referência da variável na memória.</param>
        /// <param name="tamanho">Quantidade de valores de ponto flutuante com precisão simples de 32-bit inicializados em 0.0 (zero) a serem armazenados.</param>
        public MipsDataFloat(string etiqueta, int tamanho = 1)
        {
            this.Etiqueta = etiqueta;
            this.Valores = new float[tamanho];
        }

        /// <summary>
        /// Obtém a etiqueta para criar a referência da variável na memória.
        /// </summary>
        public string Etiqueta { get; }

        /// <summary>
        /// Obtém os valores armazenados.
        /// </summary>
        public float[] Valores { get; }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty, val;
            for (int i = 0; i < this.Valores.Length; i++)
            {
                val = this.Valores[i].ToString(CultureInfo.InvariantCulture);
                if (!val.Contains('.'))
                    val += ".0";

                tmp += val;
                if (i < this.Valores.Length - 1)
                    tmp += ", ";
            }
            return $"{this.Etiqueta}: .float {tmp}";
        }
    }
}
