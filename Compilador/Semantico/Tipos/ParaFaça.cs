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
    /// Classe para o comando do tipo 'Para-Faça'.
    /// </summary>
    public class ParaFaça : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        /// <param name="listaTiposEscopoMaior">Elementos do tipo comando em escopos seguintes à este comando.</param>
        public ParaFaça(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 9)
                throw new Exception("Um tipo 'para-faça' deve receber no mínimo 9 elementos!");

            this.Elementos = elementos.ToArray();
            this.ListaTiposEscopoMaior = listaTiposEscopoMaior.ToArray();

            this.VariávelControle = this.Elementos[1];

            List<SemanticoToken> elem = new List<SemanticoToken>();
            for (int i = 3; i < this.Elementos.Length - 3; i++)
            {
                if (this.Elementos[i].Token.Equals("FACA"))
                    break;

                if (this.Elementos[i].Token.Equals("DECRESCENTE"))
                {
                    this.Decresente = true;
                    continue;
                }

                if (this.Elementos[i].Token.Equals("ATE"))
                {
                    this.ValorInicial = new Expressão(elem);
                    elem = new List<SemanticoToken>();
                    continue;
                }

                elem.Add(this.Elementos[i]);
            }

            this.ValorFinal = new Expressão(elem);
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém os elementos do tipo comando em escopos seguintes à este comando.
        /// </summary>
        public ITipo[] ListaTiposEscopoMaior { get; }

        /// <summary>
        /// Obtém o token da variável de controle.
        /// </summary>
        public SemanticoToken VariávelControle { get; }

        /// <summary>
        /// Obtém a expressão aritmética do valor inicial da variável de controle.
        /// </summary>
        public Expressão ValorInicial { get; }

        /// <summary>
        /// Obtém a expressão aritmética do valor final da variável de controle.
        /// </summary>
        public Expressão ValorFinal { get; }

        /// <summary>
        /// Obtém o modo do incremento da variável de controle (decresente se verdadeiro; cresente se falso).
        /// </summary>
        public bool Decresente { get; } = false;

        #endregion

        #region Validação Semântica

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarVariáveis(List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            // Verifica se a variável que recebe a operação foi declarada localmente:
            Variável identificador = listaVariáveisLocal.Find(x => x.Identificador.Equals(this.VariávelControle.Lexema));
            if (identificador == null)  // Não encontrada no escopo local, então:
            {
                // Busca na lista de escopo global:
                identificador = listaVariáveisGlobal.Find(x => x.Identificador.Equals(this.VariávelControle.Lexema));
                if (identificador == null)
                {
                    listaErrors.Add(new SemanticoError(this.VariávelControle, "A variável de controle informada não está declarada!"));
                    debug.Items.Add($"X > {this.VariávelControle.ToString()}");
                    //return;
                    goto ValidarBloco;
                }
            }

            if (identificador.Tipo == VariávelTipo.Lógico)
            {
                listaErrors.Add(new SemanticoError(this.VariávelControle, "A variável de controle não pode ser do tipo lógico!"));
                debug.Items.Add($"X > {this.VariávelControle.ToString()}");
                //return;
                goto ValidarBloco;
            }

            if (identificador.Tipo == VariávelTipo.Real)
            {
                listaErrors.Add(new SemanticoError(this.VariávelControle, "A variável de controle não pode ser do tipo real!"));
                debug.Items.Add($"X > {this.VariávelControle.ToString()}");
                //return;
                goto ValidarBloco;
            }

            if (identificador.Tipo == VariávelTipo.Caracter)
            {
                listaErrors.Add(new SemanticoError(this.VariávelControle, "A variável de controle não pode ser do tipo caracter!"));
                debug.Items.Add($"X > {this.VariávelControle.ToString()}");

                goto ValidarBloco;
            }

            debug.Items.Add($"> {identificador.ToString()}");

            // Variável de controle agora possui valor atribuido permanentemente, então marca:
            identificador.ValorAtribuido = true;

            this.VariávelControle.Variável = identificador;

            // Executa a validação semântica para a expressão do valor inicial da variável de controle.
            this.ValorInicial.ValidarExpressão(identificador.Tipo, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);

            // Executa a validação semântica para a expressão do valor final da variável de controle.
            this.ValorFinal.ValidarExpressão(identificador.Tipo, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);

            ValidarBloco:
            base.VerificarVariáveisComandos(listaVariáveisGlobal, listaVariáveisLocal, this.ListaTiposEscopoMaior, listaErrors, debug);
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            // Gera as instruções para expressão do valor inicial.
            this.ValorInicial.GerarMips(mips, VariávelTipo.Inteiro);

            // Move o resultado da expressão do registrador $t0 para o segmento de dados da etiqueta (variável de controle).
            mips.SectionText.Adicionar(new MipsText("sw", $"$t0,{this.VariávelControle.ObterEtiqueta()}"));

            // Move o valor da variável de controle armazenada no registrador $t0 para o registrador $t3
            mips.SectionText.Adicionar(new MipsText("move", "$t3,$t0"));

            // Adiciona a etiqueta para inicio do laço de repetição.
            mips.SectionText.Adicionar(new MipsText($"paraFacaIni_{this.Elementos[0].Index}:", null));

            // Gera as instruções para expressão do valor de final do laço.
            this.ValorFinal.GerarMips(mips, VariávelTipo.Inteiro);
            
            // Teste da condição de repetição do laço (causa a quebra do laço de repetição).
            if (this.Decresente)
                mips.SectionText.Adicionar(new MipsText("blt", $"$t3,$t0,paraFacaFim_{this.Elementos[0].Index}")); // Se decresente, quebra quando variável de controle ser menor.
            else
                mips.SectionText.Adicionar(new MipsText("bgt", $"$t3,$t0,paraFacaFim_{this.Elementos[0].Index}")); // Se cresente, quebra quando variável de controle ser maior.

            mips.SectionText.Adicionar(MipsText.Blank);

            // Adiciona os comandos dentro do laço de repatição:
            foreach (var comando in this.ListaTiposEscopoMaior)
                comando.GerarMips(mips);

            // Carrega o valor da variável de controle no registrador $t3
            mips.SectionText.Adicionar(new MipsText("lw", $"$t3,{this.VariávelControle.ObterEtiqueta()}"));

            // Adiciona o incremento/decremento da variável de controle:
            if (this.Decresente)
                mips.SectionText.Adicionar(new MipsText("subi", "$t3,$t3,1"));   // Decrementa o valor da variável de controle
            else
                mips.SectionText.Adicionar(new MipsText("addi", "$t3,$t3,1"));   // Incrementa o valor da variável de controle

            // Salva o novo valor da variável de controle do registrador $t3 para o segmento de dados da etiqueta (variável).
            mips.SectionText.Adicionar(new MipsText("sw", $"$t3,{this.VariávelControle.ObterEtiqueta()}"));

            mips.SectionText.Adicionar(new MipsText("j", $"paraFacaIni_{this.Elementos[0].Index}"));
            mips.SectionText.Adicionar(new MipsText($"paraFacaFim_{this.Elementos[0].Index}:", null));
            mips.SectionText.Adicionar(MipsText.Blank);
        }

        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar os nós subsequentes.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó do Comando
            TreeNode node = treeNode.Nodes.Add(this.ToString());

            if (this.ListaTiposEscopoMaior != null)
            {
                foreach (var comando in this.ListaTiposEscopoMaior)
                    comando.GerarArvore(node);
            }
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string inicial = String.Empty, final = String.Empty;
            foreach (var item in this.ValorInicial.Elementos)
                inicial += $"{item.Lexema} ";

            foreach (var item in this.ValorInicial.Elementos)
                final += $"{item.Lexema} ";

            return $"Para-Faça: '{this.VariávelControle.Lexema} <- {inicial}{(this.Decresente ? "decresente " : String.Empty)}até {final.Trim()}', {(this.ListaTiposEscopoMaior == null ? 0 : this.ListaTiposEscopoMaior.Count())} comandos";
        }
    }
}
