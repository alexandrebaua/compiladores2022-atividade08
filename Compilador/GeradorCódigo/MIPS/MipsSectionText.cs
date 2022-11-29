using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS
{
    /// <summary>
    /// Classe do segmento de texto (instruções) do código Assembly MIPS.
    /// </summary>
    public class MipsSectionText
    {
        #region Variáveis Privadas
        List<MipsText> _sectionText = new List<MipsText>();
        #endregion

        /// <summary>
        /// Obtém as instruções armazenadas no segmento de texto.
        /// </summary>
        public List<MipsText> SectionText { get { return this._sectionText; } }

        /// <summary>
        /// Adicionar uma nova instrução no segmento de texto.
        /// </summary>
        /// <param name="text">A instrução à adicionar.</param>
        public void Adicionar(MipsText text)
        {
            this._sectionText.Add(text);
        }

        /// <summary>
        /// Adicionar um conjunto de instruções no segmento de texto.
        /// </summary>
        /// <param name="lista">As instruções à adicionar.</param>
        public void Adicionar(MipsText[] lista)
        {
            this._sectionText.AddRange(lista);
        }

        /// <summary>
        /// Adicionar um conjunto de instruções no segmento de texto.
        /// </summary>
        /// <param name="lista">As instruções à adicionar.</param>
        public void Adicionar(List<MipsText> lista)
        {
            this._sectionText.AddRange(lista);
        }

        /// <summary>
        /// Remover um conjunto de instruções no segmento de texto.
        /// </summary>
        /// <param name="lista">As instruções à remover.</param>
        public void Remover(MipsText[] lista)
        {
            foreach (var item in lista)
                this._sectionText.Remove(item);
        }

        /// <summary>
        /// Obtém o código Assembly MIPS que representa o objeto atual.
        /// </summary>
        public string ObterCódigo()
        {
            string strText = $".text{Environment.NewLine}";

            foreach (var text in this._sectionText)
            {
                if (String.IsNullOrWhiteSpace(text.Instrução))
                {
                    strText += Environment.NewLine;
                    continue;
                }

                if (!(text.Instrução.StartsWith(".") || text.Instrução.EndsWith(":") || text.Instrução.StartsWith("#")))
                    strText += "  ";

                strText += $"{text}{Environment.NewLine}";
            }

            return strText;
        }
    }
}
