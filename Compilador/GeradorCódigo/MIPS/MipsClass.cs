using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos;

namespace Compilador.GeradorCódigo.MIPS
{
    /// <summary>
    /// Classe principal do gerador de código fonte Assembly MIPS.
    /// </summary>
    public class MipsClass
    {
        #region Variáveis Privadas
        private Algoritmo _algoritmo;
        private MipsSectionData _sectionData = new MipsSectionData();
        private MipsSectionText _sectionText = new MipsSectionText();
        private MipsLógico _handlerLogico = null;
        private MipsOptional _handlerOpcionais = null;
        private MipsStackPointer _handlerSP = null;
        #endregion

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="algoritmo">O algoritmo para geração do código.</param>
        public MipsClass(Algoritmo algoritmo)
        {
            this._algoritmo = algoritmo;
            this._handlerLogico = new MipsLógico(this);
            this._handlerOpcionais = new MipsOptional(this);
            this._handlerSP = new MipsStackPointer(this);
        }

        /// <summary>
        /// Obtém o algoritmo carregado no gerador.
        /// </summary>
        public Algoritmo Algoritmo { get { return this._algoritmo; } }

        /// <summary>
        /// Obtém o segmento de dados do código Assembly MIPS.
        /// </summary>
        public MipsSectionData SectionData { get { return this._sectionData; } }

        /// <summary>
        /// Obtém o segmento de texto (instruções) do código Assembly MIPS.
        /// </summary>
        public MipsSectionText SectionText { get { return this._sectionText; } }

        /// <summary>
        /// Obtém o manipulador auxiliar para variáveis lógicas booleanas.
        /// </summary>
        public MipsLógico HandlerLogico { get { return this._handlerLogico; } }

        /// <summary>
        /// Obtém o manipulador auxiliar para elementos opcionais utilizadas pelo gerador.
        /// </summary>
        public MipsOptional HandlerOpcionais { get { return this._handlerOpcionais; } }

        /// <summary>
        /// Obtém o manipulador auxiliar para controle do stack pointer utilizado pelo gerador.
        /// </summary>
        public MipsStackPointer HandlerStackPointer { get { return this._handlerSP; } }

        /// <summary>
        /// Obtém o código Assembly MIPS que representa o objeto atual.
        /// </summary>
        public string ObterCódigo()
        {
            // Gera o código Assembly MIPS:
            this._algoritmo.GerarMips(this);

            // Remove constantes de texto repetidas:
            List<IMipsData> secDataAsciiz = this._sectionData.SectionData.Where(x => x is MipsDataAsciiz && x.Etiqueta.StartsWith("constCar_")).ToList();
            this.OtimizarTexto(secDataAsciiz);

            // Remove constantes de texto repetidas nos comandos 'escreva':
            secDataAsciiz = this._sectionData.SectionData.Where(x => x is MipsDataAsciiz && x.Etiqueta.StartsWith("escreva_")).ToList();
            this.OtimizarTexto(secDataAsciiz);

            // Gera o texto do código Assembly MIPS resultante do código fonte do algoritmo:
            string strData = this._sectionData.ObterCódigo();
            if (strData == null)
            {
                strData = this._handlerLogico.ObterCódigo();
                if (strData != null)
                    strData = $".data{Environment.NewLine}{strData}";
            }
            else
            {
                strData += this._handlerLogico.ObterCódigo();
            }

            if (strData == null)
                return this._sectionText.ObterCódigo();

            return $"{strData}{Environment.NewLine}{this._sectionText.ObterCódigo()}";
        }

        /// <summary>
        /// Executa a otimização de constantes de texto, utilizando uma lista de resultados passada.
        /// </summary>
        /// <param name="secDataAsciiz">A lista de constantes à otimizar.</param>
        private void OtimizarTexto(List<IMipsData> secDataAsciiz)
        {
            // Se a lista estiver vazia, então retorna:
            if (secDataAsciiz.Count() == 0)
                return;
            
            // Percorre os elementos da lista:
            for (int i = 0; i < secDataAsciiz.Count(); i++)
            {
                // Armazena o elemento atual.
                MipsDataAsciiz atual = (MipsDataAsciiz)secDataAsciiz.ElementAt(i);

                // Busca por textos iguais ao texto do elemento atual.
                IEnumerable<IMipsData> repetidos = secDataAsciiz.Where(x => ((MipsDataAsciiz)x).Texto.Equals(atual.Texto) && !x.Etiqueta.Equals(atual.Etiqueta));

                // Enquanto existir textos repetidos, então:
                while (repetidos.Count() > 0)
                {
                    // Obtém o elemento repetido atual.
                    IMipsData repetido = repetidos.First();

                    // Busca por referências da etiqueta dentro da seção de texto.
                    IEnumerable<MipsText> renomear = this._sectionText.SectionText.Where(x => x.Argumentos != null && x.Argumentos.Contains($",{repetido.Etiqueta}"));

                    // Renomeia qualquer referência encontrada:
                    foreach (var ren in renomear)
                        ren.Argumentos = ren.Argumentos.Replace(repetido.Etiqueta, atual.Etiqueta);

                    // Remove a etiqueta da lista de otimização e da seção de dados:
                    secDataAsciiz.Remove(repetido);
                    this._sectionData.SectionData.Remove(repetido);
                }
            }
        }
    }
}
