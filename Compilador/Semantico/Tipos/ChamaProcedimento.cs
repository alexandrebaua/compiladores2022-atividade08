using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.GeradorCódigo.MIPS;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Chamada de Procedimento'.
    /// </summary>
    public class ChamaProcedimento : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public ChamaProcedimento(List<SemanticoToken> elementos)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 5)
                throw new Exception("Um tipo 'chama procedimento' deve receber no mínimo 5 elementos!");

            this.Elementos = elementos.ToArray();

            this.Nome = this.Elementos[0];

            List<SemanticoToken> argumentos = new List<SemanticoToken>();
            for (int i = 2; i < this.Elementos.Length - 2; i += 2)
                argumentos.Add(this.Elementos[i]);

            this.Argumentos = argumentos.ToArray();
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém o token do nome do procedimento a ser chamado.
        /// </summary>
        public SemanticoToken Nome { get; }

        /// <summary>
        /// Obtém os argumentos à serem passados para o procedimento.
        /// </summary>
        public SemanticoToken[] Argumentos { get; }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            Procedimento decProc = (Procedimento)this.Nome.Variável.Declaração;
            List<MipsText> prProcPonteiro = new List<MipsText>();
            List<string> prFuncPonteiroLog = new List<string>();
            List<MipsText> prProcPonteiroCar = new List<MipsText>();

            // Transfere o valor dos argumentos para os parâmetros do procedimento:
            int pr = 0, id = 0;
            for (int i = 0; i < this.Argumentos.Length; i++)
            {
                SemanticoToken decProcSem = decProc.Parametros[pr].Identificadores[id];

                if (this.Argumentos[i].Variável.Tipo == VariávelTipo.Lógico)
                {
                    mips.HandlerLogico.ObterVariável("$t0", this.Argumentos[i].ObterEtiqueta());  // Carrega o valor da variável no registrador $t0.

                    // Este comando é para mover o inteiro do registrador $t0 para o segmento de dados da etiqueta (variável).
                    mips.HandlerLogico.SetarVariável("$t0", decProcSem.ObterEtiqueta());

                    // Se for um parâmetro de referência por ponteiro, armazena os comandos para transferir os valores de volta aos argumentos:
                    if (decProc.Parametros[pr].Ponteiro)
                    {
                        prFuncPonteiroLog.Add(decProcSem.ObterEtiqueta());          // Carrega o valor da variável do parâmetro para o registrador $t5
                        prFuncPonteiroLog.Add(this.Argumentos[i].ObterEtiqueta());  // Move o valor do registrador $t5 para o segmento de dados da etiqueta (variável do argumento).
                    }
                    else
                    {
                        mips.HandlerStackPointer.Adicionar(decProcSem);
                    }
                }
                else if (this.Argumentos[i].Variável.Tipo == VariávelTipo.Inteiro)
                {
                    mips.SectionText.Adicionar(new MipsText("lw", $"$t0,{this.Argumentos[i].ObterEtiqueta()}"));  // Carrega o valor da variável no registrador $t0

                    if (decProcSem.Variável.Tipo == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("mtc1", "$t0,$f0"));     // Move o valor do registrador temporário $t0 para o registrador de ponto flutuante $f0
                        mips.SectionText.Adicionar(new MipsText("cvt.s.w", "$f0,$f0"));  // Converte o valor armazenado no registrador $f0 de inteiro para ponto flutuante

                        // Este comando é para mover o real do coprocessador 1 $f0 para o segmento de dados da etiqueta (variável).
                        mips.SectionText.Adicionar(new MipsText("swc1", $"$f0,{decProcSem.ObterEtiqueta()}"));
                    }
                    else
                    {
                        // Este comando é para mover o inteiro do registrador $t0 para o segmento de dados da etiqueta (variável).
                        mips.SectionText.Adicionar(new MipsText("sw", $"$t0,{decProcSem.ObterEtiqueta()}"));

                        // Se for um parâmetro de referência por ponteiro, armazena os comandos para transferir os valores de volta aos argumentos:
                        if (decProc.Parametros[pr].Ponteiro)
                        {
                            prProcPonteiro.Add(new MipsText("lw", $"$t0,{decProcSem.ObterEtiqueta()}"));          // Carrega o valor da variável do parâmetro para o registrador $t0
                            prProcPonteiro.Add(new MipsText("sw", $"$t0,{this.Argumentos[i].ObterEtiqueta()}"));  // move o valor do registrador $t0 para o segmento de dados da etiqueta (variável do argumento).
                        }
                    }
                }
                else if (this.Argumentos[i].Variável.Tipo == VariávelTipo.Real)
                {
                    mips.SectionText.Adicionar(new MipsText("lwc1", $"$f0,{this.Argumentos[i].ObterEtiqueta()}"));  // Carrega o valor da variável no registrador $f0

                    // Este comando é para mover o real do coprocessador 1 $f0 para o segmento de dados da etiqueta (variável).
                    mips.SectionText.Adicionar(new MipsText("swc1", $"$f0,{decProcSem.ObterEtiqueta()}"));

                    // Se for um parâmetro de referência por ponteiro, armazena os comandos para transferir os valores de volta aos argumentos:
                    if (decProc.Parametros[pr].Ponteiro)
                    {
                        prProcPonteiro.Add(new MipsText("lwc1", $"$f0,{decProcSem.ObterEtiqueta()}"));          // Carrega o valor da variável do parâmetro para o registrador $t0
                        prProcPonteiro.Add(new MipsText("swc1", $"$f0,{this.Argumentos[i].ObterEtiqueta()}"));  // move o valor do registrador $t0 para o segmento de dados da etiqueta (variável do argumento).
                    }
                }
                else if (this.Argumentos[i].Variável.Tipo == VariávelTipo.Caracter)
                {
                    mips.SectionText.Adicionar(new MipsText("la", $"$a0,{this.Argumentos[i].ObterEtiqueta()}"));

                    if (decProc.Parametros[pr].Ponteiro)
                    {
                        mips.SectionText.Adicionar(new MipsText("sw", $"$a0,{decProcSem.ObterEtiqueta()}"));
                    }
                    else
                    {
                        mips.SectionText.Adicionar(new MipsText("la", $"$a1,{decProcSem.ObterEtiqueta()}"));
                        mips.SectionText.Adicionar(new MipsText("jal", "funcTextoMove")); // Adiciona a chamada da função.
                    }
                }

                id++;
                if (id >= decProc.Parametros[pr].Identificadores.Length)
                {
                    pr++;
                    id = 0;
                }
            }

            mips.HandlerStackPointer.IncrementaContexto();

            mips.SectionText.Adicionar(new MipsText("jal", $"{this.Nome.Lexema}")); // Adiciona a chamada do procedimento.

            // Se existirem parâmetros por ponteiro, transfere os valores para as variáveis de argumento.
            if (prProcPonteiro.Count > 0)
                mips.SectionText.Adicionar(prProcPonteiro);

            if (prFuncPonteiroLog.Count > 0)
            {
                for (int i = 0; i < prFuncPonteiroLog.Count; i++)
                {
                    mips.HandlerLogico.ObterVariável("$t0", prFuncPonteiroLog[i++]);
                    mips.HandlerLogico.SetarVariável("$t0", prFuncPonteiroLog[i]);
                }
            }

            if (prProcPonteiroCar.Count > 0)
                mips.SectionText.Adicionar(prProcPonteiroCar);

            mips.HandlerStackPointer.DecrementaContexto();

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
            for (int i = 0; i < this.Argumentos.Length; i++)
            {
                tmp += this.Argumentos[i].Lexema;
                if (i < this.Argumentos.Length - 1)
                    tmp += ", ";
            }

            return $"Chama Procedimento: '{this.Nome.Lexema}({tmp})', {this.Argumentos.Length} argumento";
        }
    }
}
