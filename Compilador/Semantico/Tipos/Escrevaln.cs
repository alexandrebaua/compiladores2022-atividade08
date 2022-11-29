using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.GeradorCódigo.MIPS;
using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Escreva Linha'.
    /// </summary>
    public class Escrevaln : Escreva
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public Escrevaln(List<SemanticoToken> elementos) : base(elementos) { }

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            base.GerarMips(mips);

            mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));     // Comando para imprimir um texto
            mips.SectionText.Adicionar(new MipsText("la", "$a0,lfeed")); // Carregando a quebra de linha no argumento para habilitar a impressão
            mips.SectionText.Adicionar(MipsText.Syscall);                // Executando o comando
            mips.SectionText.Adicionar(MipsText.Blank);

            // Informa opcionais (quebra de linha).
            mips.HandlerOpcionais.QuebraLinha = true;
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty;
            for (int i = 0; i < this.Argumentos.Length; i++)
            {
                tmp += this.Argumentos[i].ToString();
                if (i < this.Argumentos.Length - 1)
                    tmp += ", ";
            }

            return $"Escrevaln: {tmp}";
        }
    }
}
