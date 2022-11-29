using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Exceptions;
using Compilador.GeradorCódigo.MIPS;
using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos.Auxiliar;
using Compilador.Sintatico.Ascendente.SLR;

namespace Compilador.Semantico.Tipos
{
    public enum TipoAtribuição : byte
    {
        Desconhecido = 0x00,
        Lógica = 0x01,
        Numérica = 0x02
    }

    /// <summary>
    /// Classe para o comando do tipo 'Atribuição'.
    /// </summary>
    public class Atribuição : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public Atribuição(List<SemanticoToken> elementos)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 4)
                throw new Exception("Um tipo 'atribuição' deve receber no mínimo 4 elementos!");

            this.Elementos = elementos.ToArray();
            
            List<SemanticoToken> elem = new List<SemanticoToken>();
            int i = 0;
            while (true)
            {
                elem.Add(this.Elementos[i++]);
                if (this.Elementos[i].Token.Equals("ATRIBUICAO"))
                    break;
            }
            this.Resultado = elem.ToArray();

            i++;
            elem = new List<SemanticoToken>();
            while (i < this.Elementos.Length - 1)
            {
                elem.Add(this.Elementos[i++]);
            }
            this.Expressão = new Expressão(elem);

            if (this.Elementos.Last().ReduceType == ReduceTypeEnum.AtribuiçãoLogica)
            {
                this.TipoAtribuição = TipoAtribuição.Lógica;
            }
            else
            {
                var identificador = elementos.Find(x => x.Token.Equals("CONST_INT") || x.Token.Equals("CONST_REAL"));
                if (identificador != null)
                    this.TipoAtribuição = TipoAtribuição.Numérica;
            }
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém ou define o tipo de atribuição.
        /// </summary>
        public TipoAtribuição TipoAtribuição { get; set; } = TipoAtribuição.Desconhecido;

        /// <summary>
        /// Obtém ou define o tipo do resultado da expressão.
        /// </summary>
        public VariávelTipo TipoResultado { get; set; } = VariávelTipo.Desconhecido;

        /// <summary>
        /// Obtém a variável que será atribuido o resultado da expressão.
        /// </summary>
        public SemanticoToken[] Resultado { get; }

        /// <summary>
        /// Obtém a expressão (aritmética, lógica ou texto) da atribuição.
        /// </summary>
        public Expressão Expressão { get; }

        #endregion

        #region Validação Semântica da Atribuição

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisAtribuidas">A lista de variáveis que receberam atribuição de valores (reseta quando retorna do escopo que recebeu a atribuição).</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarAtribuição(List<Variável> listaVariáveisAtribuidas, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            // Verifica se a variável que recebe a operação foi declarada localmente:
            Variável identificador = listaVariáveisLocal.Find(x => x.Identificador.Equals(this.Resultado[0].Lexema));
            if (identificador == null)  // Não encontrada no escopo local, então:
            {
                // Busca na lista de escopo global:
                identificador = listaVariáveisGlobal.Find(x => x.Identificador.Equals(this.Resultado[0].Lexema));
                if (identificador == null)
                {
                    //throw new Exception($"A variável '{this.Variável.Lexema}' foi usada mas não está declarada!");
                    listaErrors.Add(new SemanticoError(this.Resultado[0], "A variável usada não está declarada!"));
                    debug.Items.Add($"X => {this.Resultado.ToString()}");
                    return;
                }
            }

            switch (identificador.TipoDeclaração)
            {
                case VariávelDeclaração.Simples:
                    debug.Items.Add($"=> {identificador.ToString()}");
                    break;

                case VariávelDeclaração.Vetor:
                    debug.Items.Add($"=> {identificador.ToString()} : >>Vetor<<");
                    ((DeclaraçãoVetor)identificador.Declaração).ValidarIndice(this.Elementos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    break;

                case VariávelDeclaração.Procedimento:
                    listaErrors.Add(new SemanticoError(identificador, "Um procedimento não pode receber valores por atribuição!"));
                    debug.Items.Add($"X => {this.Resultado.ToString()}");
                    return;

                case VariávelDeclaração.Função:
                    listaErrors.Add(new SemanticoError(identificador, "Uma função não pode receber valores por atribuição!"));
                    debug.Items.Add($"X => {this.Resultado.ToString()}");
                    return;

                case VariávelDeclaração.Parametro:
                    debug.Items.Add($"=> {identificador.ToString()}");
                    break;

                default:
                    listaErrors.Add(new SemanticoError(identificador, $"O identificador '{identificador.Token.Lexema}' não reconhecido para receber valores por atribuição!"));
                    return;
            }

            // Armazena a referência da variável do identificador.
            this.Resultado[0].Variável = identificador;
            this.TipoResultado = identificador.Tipo;

            // Executa a validação semântica da expressão.
            this.Expressão.ValidarExpressão(identificador.Tipo, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);

            if (!identificador.ValorAtribuido)
                listaVariáveisAtribuidas.Add(identificador);

            // Variável que recebe a operação agora possui valor atribuido, então marca:
            identificador.ValorAtribuido = true;
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            this.Expressão.GerarMips(mips, this.TipoResultado);  // Adiciona os comandos da expressão na atribuição.
            this.GerarMipsAtribui(mips);  // Adiciona os comandos para mover o resultado para a variável.
        }

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para atribuir o valor do registrador para uma variável.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        public void GerarMipsAtribui(MipsClass mips)
        {
            if (this.Resultado[0].Variável.TipoDeclaração == VariávelDeclaração.Vetor)
            {
                DeclaraçãoVetor decVet = (DeclaraçãoVetor)this.Resultado[0].Variável.Declaração;  // Armazena a declaração do vetor.
                decVet.GerarMipsValorIndex(mips, this.Resultado);   // Posiciona o registro do vetor.

                switch (this.Resultado[0].Variável.Tipo)
                {
                    case VariávelTipo.Lógico:
                        // Move o resultado lógico fornecido do registrador $t4 para o registro no array
                        mips.HandlerLogico.SetaVetorValor("$t4", this.Resultado[0].ObterEtiqueta());
                        break;

                    case VariávelTipo.Inteiro:
                        // Move o resultado inteiro fornecido do registrador $t0 para o registro no array
                        mips.SectionText.Adicionar(new MipsText("sw", "$t0,($t5)"));
                        break;

                    case VariávelTipo.Real:
                        // Move o resultado real fornecido do coprocessador 1 $f4 para o registro no array
                        mips.SectionText.Adicionar(new MipsText("swc1", "$f4,($t5)"));
                        break;

                    case VariávelTipo.Caracter:
                        // Move o endereço do registro do vetor que irá receber o texto no registrador $a1.
                        mips.SectionText.Adicionar(new MipsText("move", "$a1,$t5"));

                        // Adiciona a chamada da função para mover o texto.
                        mips.SectionText.Adicionar(new MipsText("jal", "funcTextoMove"));
                        break;
                }

                mips.SectionText.Adicionar(MipsText.Blank);
                return;
            }

            if (this.Resultado[0].Variável.Tipo == VariávelTipo.Lógico)
            {
                // Move o resultado da expressão lógica do registrador $t0 para o segmento de dados da etiqueta (variável).
                mips.HandlerLogico.SetarVariável("$t4", this.Resultado[0].ObterEtiqueta());
            }
            else if (this.Resultado[0].Variável.Tipo == VariávelTipo.Inteiro)
            {
                // Move o resultado da expressão inteiro do registrador $t0 para o segmento de dados da etiqueta (variável).
                mips.SectionText.Adicionar(new MipsText("sw", $"$t0,{this.Resultado[0].ObterEtiqueta()}"));

                mips.HandlerStackPointer.Adicionar(this.Resultado[0]);
            }
            else if (this.Resultado[0].Variável.Tipo == VariávelTipo.Real)
            {
                // Move o resultado da expressão real fornecido do coprocessador 1 $f4 para o segmento de dados da etiqueta (variável).
                mips.SectionText.Adicionar(new MipsText("swc1", $"$f4,{this.Resultado[0].ObterEtiqueta()}"));
            }
            else if (this.Resultado[0].Variável.Tipo == VariávelTipo.Caracter)
            {
                if (this.Resultado[0].Variável.Ponteiro)
                {
                    // Armazena o endereço da variável que irá receber o texto no registrador $a1.
                    mips.SectionText.Adicionar(new MipsText("lw", $"$a1,{this.Resultado[0].ObterEtiqueta()}"));
                }
                else
                {
                    // Armazena o endereço da variável que irá receber o texto no registrador $a1.
                    mips.SectionText.Adicionar(new MipsText("la", $"$a1,{this.Resultado[0].ObterEtiqueta()}"));
                }

                // Inicializa o argumento contador da quantidade de caracteres.
                mips.SectionText.Adicionar(new MipsText("li", "$a2,0"));

                // Adiciona a chamada da função para mover o texto.
                mips.SectionText.Adicionar(new MipsText("jal", "funcTextoMove"));
            }

            mips.SectionText.Adicionar(MipsText.Blank);
        }
        
        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar o nó do comando.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó do Comando
            treeNode.Nodes.Add(this.ToString());
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty;

            foreach (var item in this.Resultado)
                tmp += item.Lexema;

            tmp += " <- ";

            foreach (var item in this.Expressão.Elementos)
                tmp += $"{item.Lexema} ";

            return $"Atribuição ({this.TipoAtribuição}): '{tmp.Trim()}'";
        }
    }
}
