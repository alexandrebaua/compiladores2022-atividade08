using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.GeradorCódigo.MIPS.MipsData;

namespace Compilador.GeradorCódigo.MIPS
{
    /// <summary>
    /// Classe do segmento de dados do código Assembly MIPS.
    /// </summary>
    public class MipsSectionData
    {
        #region Variáveis Privadas
        List<IMipsData> _sectionData = new List<IMipsData>();
        #endregion

        /// <summary>
        /// Obtém as diretrivas armazenadas no segmento de dados.
        /// </summary>
        public List<IMipsData> SectionData { get { return this._sectionData; } }

        /// <summary>
        /// Adicionar uma nova diretiva no segmento de dados.
        /// </summary>
        /// <param name="data">A diretriva à adicionar.</param>
        public void Adicionar(IMipsData data)
        {
            this._sectionData.Add(data);
        }

        /// <summary>
        /// Obtém o código Assembly MIPS que representa o objeto atual.
        /// </summary>
        public string ObterCódigo()
        {
            if (this._sectionData.Count == 0)
                return null;

            string strData = $".data{Environment.NewLine}";

            foreach (var data in this._sectionData)
                strData += $"  {data}{Environment.NewLine}";

            return strData;
        }
    }
}
