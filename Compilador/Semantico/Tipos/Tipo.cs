using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Exceptions;
using Compilador.GeradorCódigo.MIPS;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe abstrata para os tipos de comandos disponiveis na linguagem.
    /// Possui os métodos comuns utilizados na validação semântica.
    /// </summary>
    public abstract class Tipo : ITipo
    {
        #region Validação Semântica

        /// <summary>
        /// Executa a verificação da declaração das variáveis locais e parâmetros nos procedimentos, funções e na função principal.
        /// </summary>
        /// <param name="listaVariáveisGlobal">Lista de variáveis globais.</param>
        /// <param name="parametros">Os parâmetros dos procedimentos e funções.</param>
        /// <param name="variáveis">A lista das declarações das variáveis locais.</param>
        /// <param name="comandos">A lista de comandos comandos.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a analise semântica.</param>
        /// <param name="debug">Saida de debugação.</param>
        protected void VerificarVariáveis(List<Variável> listaVariáveisGlobal, ParâmetroProcFunc[] parametros, ITipo[] variáveis, ITipo[] comandos, List<SemanticoError> listaErrors, ListBox debug)
        {
            List<Variável> listaVariáveisLocal = new List<Variável>();
            
            // Se existirem parâmetros informados no procedimento ou função, então testa:
            if (parametros != null && parametros.Length > 0)
            {
                foreach (var itemPr in parametros)
                {
                    foreach (var item in itemPr.ObterVariáveis())
                    {
                        if (listaVariáveisGlobal != null)
                        {
                            if (listaVariáveisGlobal.Find(x => x.Identificador.Equals(item.Identificador)) != null)
                            {
                                listaErrors.Add(new SemanticoError(item, "Variável global com o mesmo nome do parâmetro!"));
                                debug.Items.Add($"X > {item.ToString()}");
                                continue;
                            }
                        }
                        
                        if (listaVariáveisLocal.Find(x => x.Identificador.Equals(item.Identificador)) != null)
                        {
                            listaErrors.Add(new SemanticoError(item, "Parâmetro repetido!"));
                            debug.Items.Add($"X > {item.ToString()}");
                            continue;
                        }

                        listaVariáveisLocal.Add(item);
                        debug.Items.Add($"> {item.ToString()}");
                    }
                }
            }

            // Se existirem variáveis locais, então testa:
            if (variáveis != null && variáveis.Length > 0)
            {
                foreach (var itemVar in variáveis)
                {
                    if (itemVar is DeclaraçãoSimples)
                    {
                        DeclaraçãoSimples varS = (DeclaraçãoSimples)itemVar;
                        foreach (var item in varS.ObterVariáveis())
                        {
                            if (listaVariáveisGlobal != null)
                            {
                                if (listaVariáveisGlobal.Find(x => x.Identificador.Equals(item.Identificador)) != null)
                                {
                                    listaErrors.Add(new SemanticoError(item, "Variável com o mesmo nome com declaração global!"));
                                    debug.Items.Add($"X > {item.ToString()}");
                                    continue;
                                }
                            }

                            Variável varLocal = listaVariáveisLocal.Find(x => x.Identificador.Equals(item.Identificador));
                            if (varLocal != null)
                            {
                                listaErrors.Add(new SemanticoError(item, varLocal.TipoDeclaração == VariávelDeclaração.Parametro ? "Variável com o mesmo nome declarado como parâmetro!" : "Variável com o mesmo nome já declarado local!"));
                                debug.Items.Add($"X > {item.ToString()}");
                                continue;
                            }

                            listaVariáveisLocal.Add(item);
                            debug.Items.Add($"> {item.ToString()}");
                        }

                        continue;
                    }

                    Variável varV = ((DeclaraçãoVetor)itemVar).ObterVariável();
                    if (listaVariáveisGlobal != null)
                    {
                        if (listaVariáveisGlobal.Find(x => x.Identificador.Equals(varV.Identificador)) != null)
                        {
                            listaErrors.Add(new SemanticoError(varV, "Variável com o mesmo nome com declaração global!"));
                            debug.Items.Add($"X > {varV.ToString()}");
                            continue;
                        }
                    }
                    
                    if (listaVariáveisLocal.Find(x => x.Identificador.Equals(varV.Identificador)) != null)
                    {
                        listaErrors.Add(new SemanticoError(varV, "Variável com o mesmo nome com declaração local!"));
                        debug.Items.Add($"X > {varV.ToString()}");
                        continue;
                    }

                    listaVariáveisLocal.Add(varV);
                    debug.Items.Add($"> {varV.ToString()}");
                }
            }

            if (listaErrors.Count > 0)
                throw new SemanticoListException(listaErrors);
            
            this.VerificarVariáveisComandos(listaVariáveisGlobal, listaVariáveisLocal, comandos, listaErrors, debug);
        }

        /// <summary>
        /// Executa a verificação das variáveis utilizadas nos comandos.
        /// </summary>
        /// <param name="listaVariáveisGlobal">Lista de variáveis globais.</param>
        /// <param name="listaVariáveisLocal">Lista de variáveis locais.</param>
        /// <param name="comandos">A lista de comandos comandos.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a analise semântica.</param>
        /// <param name="debug">Saida de debugação.</param>
        protected void VerificarVariáveisComandos(List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, ITipo[] comandos, List<SemanticoError> listaErrors, ListBox debug)
        {
            List<Variável> listaVariáveisAtribuidas = new List<Variável>();

            // Verifica os comandos no escopo, e testa qualquer variável utilizada:
            foreach (var comando in comandos)
            {
                Type tipoComando = comando.GetType();
                if (tipoComando == typeof(Atribuição))
                {
                    ((Atribuição)comando).ValidarAtribuição(listaVariáveisAtribuidas, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                }
                else if (tipoComando == typeof(ChamaProcedimento))
                {
                    var comProc = (ChamaProcedimento)comando;
                    debug.Items.Add($"-------> {comProc.ToString()}");

                    // Busca na lista de escopo global:
                    Variável comProcVar = listaVariáveisGlobal.Find(x => x.Identificador.Equals(comProc.Nome.Lexema));
                    if (comProcVar == null)
                    {
                        listaErrors.Add(new SemanticoError(comProc, "O procedimento foi usada mas não foi declarado!"));
                        debug.Items.Add($"X > {comProc.ToString()}");
                        continue;
                    }

                    // Se for uma função não pode ser utilizada como procedimento, então lança erro:
                    if (comProcVar.TipoDeclaração == VariávelDeclaração.Função)
                    {
                        listaErrors.Add(new SemanticoError(comProc, "Uma função não pode ser utilizada como um procedimento!"));
                        debug.Items.Add($"X > {comProc.ToString()}");
                        continue;
                    }

                    // Armazena a referência da variável do procedimento, e marca o procedimento como utilizado.
                    comProc.Nome.Variável = comProcVar;
                    comProcVar.Utilizado = true;

                    ((Procedimento)comProcVar.Declaração).ValidarArgumentos(comProc, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comProc.ToString()}");
                }
                else if (tipoComando == typeof(EnquantoFaça))
                {
                    var comEnq = (EnquantoFaça)comando;
                    debug.Items.Add($"-------> {comEnq.ToString()}");
                    comEnq.VerificarVariáveis(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comEnq.ToString()}");
                }
                else if (tipoComando == typeof(RepitaAtéQue))
                {
                    var comRep = (RepitaAtéQue)comando;
                    debug.Items.Add($"-------> {comRep.ToString()}");
                    comRep.VerificarVariáveis(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comRep.ToString()}");
                }
                else if (tipoComando == typeof(ParaFaça))
                {
                    var comPara = (ParaFaça)comando;
                    debug.Items.Add($"-------> {comPara.ToString()}");
                    comPara.VerificarVariáveis(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comPara.ToString()}");
                }
                else if (tipoComando == typeof(SeSenão))
                {
                    var comSeSenao = (SeSenão)comando;
                    debug.Items.Add($"-------> {comSeSenao.ToString()}");
                    comSeSenao.VerificarVariáveis(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comSeSenao.ToString()}");
                }
                else if (tipoComando == typeof(Leia))
                {
                    var comLeia = (Leia)comando;
                    debug.Items.Add($"-------> {comLeia.ToString()}");
                    comLeia.VerificarArgumentos(listaVariáveisAtribuidas, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comLeia.ToString()}");
                }
                else if (tipoComando == typeof(Escreva))
                {
                    var comEscr = (Escreva)comando;
                    debug.Items.Add($"-------> {comEscr.ToString()}");
                    comEscr.VerificarArgumentos(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comEscr.ToString()}");
                }
                else if (tipoComando == typeof(Escrevaln))
                {
                    var comEscrLn = (Escrevaln)comando;
                    debug.Items.Add($"-------> {comEscrLn.ToString()}");
                    comEscrLn.VerificarArgumentos(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    debug.Items.Add($"<------- {comEscrLn.ToString()}");
                }
            }

            // Se esse objeto for uma função, então testa também o retorno:
            if (this is Função)
                ((Função)this).Retorne.VerificarVariáveis(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);

            // Se foi atribuido valor para alguma variável, então reseta marcador:
            if (listaVariáveisAtribuidas.Count > 0)
            {
                foreach (var item in listaVariáveisAtribuidas)
                    item.ValorAtribuido = false;
            }
        }

        /// <summary>
        /// Cria um comando baseado no tipo de redução armazenado no último elemento da lista, e associa a lista de comandos de escopo interno (se aplicável).
        /// </summary>
        /// <param name="elementos">Os elementos que compõe o comando.</param>
        /// <param name="listaTiposEscopoMaior">A lista de comandos dentro deste comando.</param>
        /// <returns>Um novo comando contendo as informações passadas.</returns>
        public static Tipo Criar(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
            switch (elementos.Last().ReduceType)
            {
                case Sintatico.Ascendente.SLR.ReduceTypeEnum.None:
                    break;
                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Algoritmo:
                    return new Algoritmo(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Procedimento:
                    return new Procedimento(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Função:
                    return new Função(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Principal:
                    return new Principal(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.SeleçãoIf:
                    return new SeSenão(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Enquanto:
                    return new EnquantoFaça(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Repita:
                    return new RepitaAtéQue(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.ParaFaça:
                    return new ParaFaça(elementos, listaTiposEscopoMaior);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Declaração:
                    break;
                case Sintatico.Ascendente.SLR.ReduceTypeEnum.AtribuiçãoNumérica:
                case Sintatico.Ascendente.SLR.ReduceTypeEnum.AtribuiçãoLogica:
                    return new Atribuição(elementos);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Leia:
                    return new Leia(elementos);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Escreva:
                    return new Escreva(elementos);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.Escrevaln:
                    return new Escrevaln(elementos);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.ChamaProcedimento:
                    return new ChamaProcedimento(elementos);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.FunçãoRetorne:
                    return new FunçãoRetorne(elementos);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.DeclaraçãoSimples:
                    return new DeclaraçãoSimples(elementos);

                case Sintatico.Ascendente.SLR.ReduceTypeEnum.DeclaraçãoVetor:
                    return new DeclaraçãoVetor(elementos);

                default:
                    break;
            }

            return null;
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Método abstrato para geração dos comandos Assembly MIPS para os componentes.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public abstract void GerarMips(MipsClass mips);

        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Método abstrato para geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">O nó base para os nós subsequentes.</param>
        public abstract void GerarArvore(TreeNode treeNode);

        #endregion
    }
}
