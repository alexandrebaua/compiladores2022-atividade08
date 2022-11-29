using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.GeradorCódigo.MIPS.MipsData
{
    /// <summary>
    /// Interface para elementos da seção de dados MIPS.
    /// </summary>
    public interface IMipsData
    {
        string Etiqueta { get; }
    }
}
