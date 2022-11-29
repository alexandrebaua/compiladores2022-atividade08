using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Exceptions;
using Compilador.GeradorCódigo.MIPS;
using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Algoritmo'.
    /// </summary>
    public class Algoritmo : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        /// <param name="listaTiposEscopoMaior">Elementos do tipo comando em escopos seguintes à este comando.</param>
        public Algoritmo(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count != 3)
                throw new Exception("Um tipo 'algoritmo' deve receber 3 elementos!");

            this.Elementos = elementos.ToArray();
            this.ListaTiposEscopoMaior = listaTiposEscopoMaior.ToArray();

            this.Nome = this.Elementos[1].Lexema;

            List<ITipo> variáveis = new List<ITipo>();
            List<ITipo> comandos = new List<ITipo>();
            foreach (var item in this.ListaTiposEscopoMaior)
            {
                if (item is DeclaraçãoSimples || item is DeclaraçãoVetor)
                    variáveis.Add(item);
                else if (item is Principal)
                    this.Principal = (Principal)item;
                else
                    comandos.Add(item);
            }

            if (variáveis.Count > 0)
                this.Variáveis = variáveis.ToArray();

            if (comandos.Count > 0)
                this.Comandos = comandos.ToArray();
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
        /// Obtém o nome do algoritmo.
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Obtém as variáveis declaradas no escopo do algoritmo (estas são variáveis globais).
        /// </summary>
        public ITipo[] Variáveis { get; } = null;

        /// <summary>
        /// Obtém os comandos que compõe o algoritmo.
        /// </summary>
        public ITipo[] Comandos { get; } = null;

        /// <summary>
        /// Obtém a função principal do algoritmo.
        /// </summary>
        public Principal Principal { get; }

        #endregion

        #region Validação Semântica

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarVariáveis(ListBox debug)
        {
            debug.Items.Add("---> Algoritmo");

            //this._arraysUtilizados = false;
            List<Variável> listaVariáveisGlobal = new List<Variável>();
            List<SemanticoError> listaErrors = new List<SemanticoError>();

            // Se existirem variáveis globais, então testa:
            if (this.Variáveis != null && this.Variáveis.Length > 0)
            {
                foreach (var itemVar in this.Variáveis)
                {
                    if (itemVar is DeclaraçãoSimples)
                    {
                        DeclaraçãoSimples varS = (DeclaraçãoSimples)itemVar;
                        foreach (var item in varS.ObterVariáveis())
                        {
                            if (listaVariáveisGlobal.Find(x => x.Identificador.Equals(item.Identificador)) != null)
                            {
                                listaErrors.Add(new SemanticoError(item, "Variável global repetida!"));
                                debug.Items.Add($"X > {item.ToString()}");
                                continue;
                            }

                            item.Global = true;
                            listaVariáveisGlobal.Add(item);
                            debug.Items.Add($"> {item.ToString()}");
                        }

                        continue;
                    }
                    
                    Variável varV = ((DeclaraçãoVetor)itemVar).ObterVariável();
                    if (listaVariáveisGlobal.Find(x => x.Identificador.Equals(varV.Identificador)) != null)
                    {
                        listaErrors.Add(new SemanticoError(varV, "Variável global repetida!"));
                        debug.Items.Add($"X > {varV.ToString()}");
                        continue;
                    }

                    varV.Global = true;
                    listaVariáveisGlobal.Add(varV);
                    debug.Items.Add($"> {varV.ToString()}");
                }
            }

            if (listaErrors.Count > 0)
                throw new SemanticoListException(listaErrors);

            // Se existirem comandos no escopo (procedimentos ou funções), então testa:
            if (this.Comandos != null && this.Comandos.Length > 0)
            {
                foreach (var item in this.Comandos)
                {
                    Type tipoItem = item.GetType();
                    if (tipoItem == typeof(Procedimento))
                    {
                        var elProc = (Procedimento)item;

                        Variável varProc = elProc.ObterVariável();
                        varProc.Global = true;

                        // Busca na lista de escopo global:
                        Variável comProcVar = listaVariáveisGlobal.Find(x => x.Identificador.Equals(elProc.Nome.Lexema));
                        if (comProcVar != null)
                        {
                            listaErrors.Add(new SemanticoError(elProc.Nome, "Já existe um procedimento com o mesmo nome!"));
                            debug.Items.Add($"X > {elProc.ToString()}");
                            continue;
                        }
                        
                        listaVariáveisGlobal.Add(varProc);
                        debug.Items.Add($"> {varProc.ToString()}");

                        debug.Items.Add($"-----> {elProc}");

                        elProc.VerificarVariáveis(listaVariáveisGlobal, listaErrors, debug);

                        debug.Items.Add($"<----- {elProc}");
                    }
                    else if (tipoItem == typeof(Função))
                    {
                        var elFunc = (Função)item;

                        Variável varFunc = elFunc.ObterVariável();
                        varFunc.Global = true;
                        listaVariáveisGlobal.Add(varFunc);
                        debug.Items.Add($"> {varFunc.ToString()}");

                        debug.Items.Add($"-----> {elFunc}");

                        elFunc.VerificarVariáveis(listaVariáveisGlobal, listaErrors, debug);

                        debug.Items.Add($"<----- {elFunc}");
                    }
                }
            }

            if (listaErrors.Count > 0)
                throw new SemanticoListException(listaErrors);

            debug.Items.Add("-----> Função Principal");

            this.Principal.VerificarVariáveis(listaVariáveisGlobal, listaErrors, debug);

            debug.Items.Add("<----- Função Principal");
            debug.Items.Add("<--- Algoritmo");

            if (listaErrors.Count > 0)
                throw new SemanticoListException(listaErrors);
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            // Se existirem variáveis globais, então:
            if (this.Variáveis != null)
            {
                // Adiciona as variáveis:
                foreach (var item in this.Variáveis)
                    item.GerarMips(mips);
            }

            mips.SectionText.Adicionar(new MipsText(".globl", "main"));
            
            // Se existirem procedimentos ou funções, então:
            if (this.Comandos != null)
            {
                mips.SectionText.Adicionar(new MipsText("j", "main"));
                mips.SectionText.Adicionar(MipsText.Blank);

                // Incrementa o contexto, para sinalizar que estará tratando de procedimentos e funções.
                mips.HandlerStackPointer.IncrementaContexto();

                foreach (var comando in this.Comandos)
                {
                    comando.GerarMips(mips);

                    // Finalizou o tratamento do procedimento ou função, descarta o contexto.
                    mips.HandlerStackPointer.DescartaContexto();
                }

                // Decrementa o contexto, para sinalizar que finalizou o tratando de procedimentos e funções.
                mips.HandlerStackPointer.DecrementaContexto();
            }

            this.Principal.GerarMips(mips);

            mips.SectionText.Adicionar(MipsText.Exit);    // Adiciona o fim de programa
            mips.SectionText.Adicionar(MipsText.Syscall); // Executando o comando
            
            // Inclui componentes opcionais necessários nas seções 'data' e 'text':
            mips.HandlerOpcionais.IncluirOpcionais();
        }
        
        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração da visualização da árvore resultante de análise semântica.
        /// </summary>
        /// <param name="treeView">Um componente do tipo árvore de visualização.</param>
        public void GerarArvore(TreeView treeView)
        {
            // Cria o Nó Raiz (RootNode)
            TreeNode rootNode = treeView.Nodes.Add(this.ToString());

            this.GerarArvore(rootNode);
        }

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar os nós subsequentes.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Se existir variáveis globais, então adiciona à árvore:
            if (this.Variáveis != null)
            {
                TreeNode varGlobal = treeNode.Nodes.Add("Variáveis Globais");
                foreach (var item in this.Variáveis)
                    varGlobal.Nodes.Add(item.ToString());
            }

            // Se existir procedimentos ou funções, então adiciona à árvore:
            if (this.Comandos != null)
            {
                foreach (var comando in this.Comandos)
                    comando.GerarArvore(treeNode);
            }

            // Adiciona a função principal à árvore:
            this.Principal.GerarArvore(treeNode);
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            int var = 0, dec = 0;
            if (this.Variáveis != null)
            {
                foreach (var item in this.Variáveis)
                {
                    if (item is DeclaraçãoVetor)
                        var += 1;
                    else if (item is DeclaraçãoSimples)
                        var += ((DeclaraçãoSimples)item).Identificadores.Length;
                }

                dec = this.Variáveis.Count();
            }

            return $"Algoritmo: {this.Nome}, {var} variável global ({dec} declaração), {(this.Comandos == null ? 0 : this.Comandos.Count())} procedimento/função";
        }
    }
}
