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
    /// Classe para o comando do tipo 'Função'.
    /// </summary>
    public class Função : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        /// <param name="listaTiposEscopoMaior">Elementos do tipo comando em escopos seguintes à este comando.</param>
        public Função(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
           if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 7)
                throw new Exception("Um tipo 'função' deve receber no mínimo 7 elementos!");

            this.Elementos = elementos.ToArray();
            this.ListaTiposEscopoMaior = listaTiposEscopoMaior.ToArray();

            this.Nome = this.Elementos[1];
            this.TipoRetorno = this.Elementos[this.Elementos.Length - 2];

            if (this.Elementos.Length >= 10)
            {
                List<ParâmetroProcFunc> parametros = new List<ParâmetroProcFunc>();
                List<SemanticoToken> elem = new List<SemanticoToken>();
                for (int i = 3; i < this.Elementos.Length - 4; i++)
                {
                    if (this.Elementos[i].Token.Equals("PONTO_VIRGULA"))
                    {
                        parametros.Add(new ParâmetroProcFunc(elem));
                        elem = new List<SemanticoToken>();
                        continue;
                    }

                    if (this.Elementos[i].Token.Equals("FECHA_PAR"))
                        break;

                    elem.Add(this.Elementos[i]);
                }
                if (elem.Count > 0)
                    parametros.Add(new ParâmetroProcFunc(elem));

                if (parametros.Count > 0)
                    this.Parametros = parametros.ToArray();
            }

            List<ITipo> variáveis = new List<ITipo>();
            List<ITipo> comandos = new List<ITipo>();
            foreach (var item in listaTiposEscopoMaior)
            {
                if (item is DeclaraçãoSimples || item is DeclaraçãoVetor)
                    variáveis.Add(item);
                else if (item is FunçãoRetorne)
                    this.Retorne = (FunçãoRetorne)item;
                else
                    comandos.Add(item);
            }

            if (variáveis.Count > 0)
                this.Variáveis = variáveis.ToArray();

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
        /// Obtém o token do nome da função.
        /// </summary>
        public SemanticoToken Nome { get; }

        /// <summary>
        /// Obtém o token do tipo de retorno da função.
        /// </summary>
        public SemanticoToken TipoRetorno { get; }

        /// <summary>
        /// Obtém os parâmetros da função.
        /// </summary>
        public ParâmetroProcFunc[] Parametros { get; } = null;

        /// <summary>
        /// Obtém as variáveis declaradas no escopo da função.
        /// </summary>
        public ITipo[] Variáveis { get; } = null;

        /// <summary>
        /// Obtém os comandos que compõe a função.
        /// </summary>
        public ITipo[] Comandos { get; }

        /// <summary>
        /// Obtém o comando de retorno da função.
        /// </summary>
        public FunçãoRetorne Retorne { get; }

        #endregion

        /// <summary>
        /// Obtém a variável referênte à esta declaração.
        /// </summary>
        public Variável ObterVariável()
        {
            return new Variável(this);
        }

        #region Validação Semântica da Função

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarVariáveis(List<Variável> listaVariáveisGlobal, List<SemanticoError> listaErrors, ListBox debug)
        {
            base.VerificarVariáveis(listaVariáveisGlobal, this.Parametros, this.Variáveis, this.Comandos, listaErrors, debug);
        }

        /// <summary>
        /// Executa a validação dos argumentos passados para os parâmetrso da função.
        /// </summary>
        /// <param name="index">Indice do elemento atual analisado na expressão.</param>
        /// <param name="lista">Lista de elementos semânticos da expressão.</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarArgumentos(ref int index, SemanticoToken[] lista, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            SemanticoToken identificadorToken = lista[index];

            if (!lista[index++].Lexema.Equals(this.Nome.Lexema))
                throw new Exception("O nome passado não corresponde ao nome da declaração.");

            if (!lista[index++].Token.Equals("ABRE_PAR"))
                throw new Exception("Esperado abre parênteses para o início dos argumentos da função.");

            debug.Items.Add($">>> {this.Nome.Lexema}(?)");

            if (this.Parametros == null)
            {
                if (!lista[index++].Token.Equals("FECHA_PAR"))
                    throw new Exception("Esperado fecha parênteses para o final dos argumentos da função.");

                debug.Items.Add($">>> {this.Nome.Lexema}(ok)");
                index++;
                return;
            }

            int errosInicio = listaErrors.Count;
            int contaArgumentos = 1;
            int argumento = 0;
            int identificador = 0;
            VariávelTipo parametroTipo = Variável.ConverterTipo(this.Parametros[0].TipoDeDados);
            bool parametroPonteiro = this.Parametros[0].Ponteiro;

            while (index < lista.Length)
            {
                SemanticoToken itemExpr = lista[index];

                if (itemExpr.Token.Equals("ID"))
                {
                    // Verifica se a variável na operação foi declarada localmente:
                    Variável elementsExpr = listaVariáveisLocal.Find(x => x.Identificador.Equals(itemExpr.Lexema));
                    if (elementsExpr == null)  // Não encontrada no escopo local, então:
                    {
                        // Busca na lista de escopo global:
                        elementsExpr = listaVariáveisGlobal.Find(x => x.Identificador.Equals(itemExpr.Lexema));
                        if (elementsExpr == null)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, "A variável usada como argumento da função não está declarada!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }
                    }

                    // Armazena a referência da variável do procedimento.
                    itemExpr.Variável = elementsExpr;

                    if (elementsExpr.TipoDeclaração == VariávelDeclaração.Vetor)
                    {
                        ((DeclaraçãoVetor)elementsExpr.Declaração).ValidarIndice(ref index, lista, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                        continue;
                    }

                    if (!elementsExpr.ValorAtribuido)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A variável usada no argumento da função não possui valor atribuido!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        index++;
                        continue;
                    }

                    if (elementsExpr.Tipo == VariávelTipo.Real)
                    {
                        if (parametroTipo == VariávelTipo.Lógico || parametroTipo == VariávelTipo.Inteiro || parametroTipo == VariávelTipo.Caracter)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' da função não pode receber uma variável do tipo real!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                    }
                    else if (elementsExpr.Tipo == VariávelTipo.Inteiro)
                    {
                        if (parametroTipo == VariávelTipo.Lógico  || parametroTipo == VariávelTipo.Caracter)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' da função não pode receber uma variável do tipo inteiro!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                        if (parametroTipo == VariávelTipo.Real && parametroPonteiro)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' da função não pode receber uma variável do tipo inteiro por ponteiro!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }
                    }
                    else if (elementsExpr.Tipo == VariávelTipo.Caracter)
                    {
                        if (parametroTipo != VariávelTipo.Caracter)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' da função não pode receber uma variável do tipo caracter!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                    }
                    else if (elementsExpr.Tipo == VariávelTipo.Lógico)
                    {
                        if (parametroTipo != VariávelTipo.Lógico)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' da função não pode receber uma variável do tipo lógico!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }
                    }
                    
                    debug.Items.Add($">> {elementsExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("VIRGULA"))
                {

                    if (++identificador >= this.Parametros[argumento].Identificadores.Length)
                    {
                        if(++argumento >= this.Parametros.Length)
                        {
                            while (index < lista.Length)
                            {
                                if (lista[index].Token.Equals("FECHA_PAR"))
                                    break;

                                if (!lista[index].Token.Equals("VIRGULA"))
                                    contaArgumentos++;

                                index++;
                            }
                            break;
                        }

                        parametroTipo = Variável.ConverterTipo(this.Parametros[argumento].TipoDeDados);
                        parametroPonteiro = this.Parametros[argumento].Ponteiro;
                    }

                    contaArgumentos++;
                }
                else if (itemExpr.Token.Equals("FECHA_PAR"))
                {
                    break;
                }

                index++;
            }
            
            int parametros = 0;
            for (int i = 0; i < this.Parametros.Length; i++)
                parametros += this.Parametros[i].Identificadores.Length;

            if (contaArgumentos != parametros)
            {
                int comprimento = lista[index].Index + lista[index].Lexema.Length - identificadorToken.Index;
                listaErrors.Add(new SemanticoError(this.Nome.Lexema, identificadorToken.Linha, identificadorToken.Inicio, identificadorToken.Index, comprimento, $"Foram passados {contaArgumentos} argumentos, mas a função possui {parametros} parâmetros."));
                debug.Items.Add($">>> {this.Nome.Lexema}(erro...)");
                return;
            }

            string tmp = "ok";
            for (int i = 1; i < contaArgumentos; i++)
                tmp += ", ok";
            debug.Items.Add($">>> {this.Nome.Lexema}({tmp})");
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            if (!this.Nome.Variável.Utilizado)
                return;

            foreach (var parametro in this.Parametros)
            {
                foreach (var id in parametro.Identificadores)
                {
                    switch (id.Variável.Tipo)
                    {
                        case VariávelTipo.Lógico:
                            mips.HandlerLogico.AdicionarVariável(id.ObterEtiqueta());
                            break;
                        case VariávelTipo.Inteiro:
                            mips.SectionData.Adicionar(new MipsDataWord(id.ObterEtiqueta()));
                            break;
                        case VariávelTipo.Real:
                            mips.SectionData.Adicionar(new MipsDataFloat(id.ObterEtiqueta()));
                            break;
                        case VariávelTipo.Caracter:
                            if (id.Variável.Ponteiro)
                                mips.SectionData.Adicionar(new MipsDataWord(id.ObterEtiqueta()));
                            else
                                mips.SectionData.Adicionar(new MipsDataSpace(id.ObterEtiqueta(), 34));
                            break;
                    }
                }
            }

            mips.SectionText.Adicionar(new MipsText("#", $"Função: '{this.InfoDeclaração()}'"));
            mips.SectionText.Adicionar(new MipsText($"{this.Nome.Lexema}:", null)); // Adiciona a etiqueta para a chamada da função.

            // Salva o enderço de retono no stack pointer:
            mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,-4"));
            mips.SectionText.Adicionar(new MipsText("sw", "$ra,0($sp)"));
            mips.SectionText.Adicionar(MipsText.Blank);

            if (this.Variáveis != null)
            {
                foreach (var variavel in this.Variáveis)
                    variavel.GerarMips(mips);
            }

            foreach (var comando in this.Comandos)
                comando.GerarMips(mips);

            this.Retorne.GerarMips(mips);

            // Restaura o enderço de retono do stack pointer:
            mips.SectionText.Adicionar(new MipsText("lw", "$ra,0($sp)"));
            mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,4"));

            mips.SectionText.Adicionar(new MipsText("jr", "$ra"));  // Volta para o lugar de onde foi chamado.
            mips.SectionText.Adicionar(MipsText.Blank);
        }

        /// <summary>
        /// Extrai os argumentos da função, e gera a chamada da função utilizando os argumentos extraidos.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="lista">A lista de elementos que compõe a expressão.</param>
        /// <param name="i">Indice do elemento atual analisado na expressão.</param>
        /// <param name="offset">Offset de indice do elemento atual analisado na expressão.</param>
        public void GerarMipsChamada(MipsClass mips, SemanticoToken[] lista, ref int i, int offset)
        {
            List<SemanticoToken> argumentos = new List<SemanticoToken>();
            i += 2;
            for (; i < lista.Length; i++)
            {
                if (lista[i + offset].Token.Equals("FECHA_PAR"))
                    break;

                if (lista[i + offset].Token.Equals("VIRGULA"))
                    continue;

                argumentos.Add(lista[i + offset]);
            }

            this.GerarMipsChamada(mips, argumentos);
        }

        /// <summary>
        /// Gera a chamada da função utilizando os argumentos passados.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="argumentos">Argumentos a serem passados para os parâmetros da função.</param>
        public void GerarMipsChamada(MipsClass mips, List<SemanticoToken> argumentos)
        {
            List<MipsText> prFuncPonteiro = new List<MipsText>();
            List<string> prFuncPonteiroLog = new List<string>();
            List<MipsText> prProcPonteiroCar = new List<MipsText>();

            // Transfere o valor dos argumentos para os parâmetros do procedimento:
            int pr = 0, id = 0;
            for (int i = 0; i < argumentos.Count; i++)
            {
                SemanticoToken decProcSem = this.Parametros[pr].Identificadores[id];

                if (argumentos[i].Variável.Tipo == VariávelTipo.Lógico)
                {
                    mips.HandlerLogico.ObterVariável("$t4", argumentos[i].ObterEtiqueta());  // Carrega o valor da variável no registrador $t4

                    // Este comando é para mover o inteiro do registrador $t4 para o segmento de dados da etiqueta (variável).
                    mips.HandlerLogico.SetarVariável("$t4", decProcSem.ObterEtiqueta());

                    // Se for um parâmetro de referência por ponteiro, armazena os comandos para transferir os valores de volta aos argumentos:
                    if (this.Parametros[pr].Ponteiro)
                    {
                        prFuncPonteiroLog.Add(decProcSem.ObterEtiqueta());     // Carrega o valor da variável do parâmetro para o registrador $t5
                        prFuncPonteiroLog.Add(argumentos[i].ObterEtiqueta());  // Move o valor do registrador $t5 para o segmento de dados da etiqueta (variável do argumento).
                    }
                }
                else if (argumentos[i].Variável.Tipo == VariávelTipo.Inteiro)
                {
                    mips.SectionText.Adicionar(new MipsText("lw", $"$t5,{argumentos[i].ObterEtiqueta()}"));  // Carrega o valor da variável no registrador $t5

                    if (decProcSem.Variável.Tipo == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("mtc1", "$t5, $f9"));     // Move o valor do registrador temporário $t0 para o registrador de ponto flutuante $f9
                        mips.SectionText.Adicionar(new MipsText("cvt.s.w", "$f9, $f9"));  // Converte o valor armazenado no registrador $f9 de inteiro para ponto flutuante

                        // Este comando é para mover o real do coprocessador 1 $f9 para o segmento de dados da etiqueta (variável).
                        mips.SectionText.Adicionar(new MipsText("swc1", $"$f9,{decProcSem.ObterEtiqueta()}"));
                    }
                    else
                    {
                        // Este comando é para mover o inteiro do registrador $t5 para o segmento de dados da etiqueta (variável).
                        mips.SectionText.Adicionar(new MipsText("sw", $"$t5,{decProcSem.ObterEtiqueta()}"));

                        // Se for um parâmetro de referência por ponteiro, armazena os comandos para transferir os valores de volta aos argumentos:
                        if (this.Parametros[pr].Ponteiro)
                        {
                            prFuncPonteiro.Add(new MipsText("lw", $"$t5,{decProcSem.ObterEtiqueta()}"));     // Carrega o valor da variável do parâmetro para o registrador $t5
                            prFuncPonteiro.Add(new MipsText("sw", $"$t5,{argumentos[i].ObterEtiqueta()}"));  // Move o valor do registrador $t5 para o segmento de dados da etiqueta (variável do argumento).
                        }
                        else
                        {
                            mips.HandlerStackPointer.Adicionar(decProcSem);
                        }
                    }
                }
                else if (argumentos[i].Variável.Tipo == VariávelTipo.Real)
                {
                    //mips.SectionText.Adicionar(new MipsText("lwc1", $"$f9, varReal_{argumentos[i].Lexema}"));  // Carrega o valor da variável no registrador $f9
                    mips.SectionText.Adicionar(new MipsText("lwc1", $"$f9,{argumentos[i].ObterEtiqueta()}"));

                    // Este comando é para mover o real do coprocessador 1 $f9 para o segmento de dados da etiqueta (variável).
                    mips.SectionText.Adicionar(new MipsText("swc1", $"$f9,{decProcSem.ObterEtiqueta()}"));

                    // Se for um parâmetro de referência por ponteiro, armazena os comandos para transferir os valores de volta aos argumentos:
                    if (this.Parametros[pr].Ponteiro)
                    {
                        prFuncPonteiro.Add(new MipsText("lwc1", $"$f9,{decProcSem.ObterEtiqueta()}"));     // Carrega o valor da variável do parâmetro para o registrador $f9
                        prFuncPonteiro.Add(new MipsText("swc1", $"$f9,{argumentos[i].ObterEtiqueta()}"));  // Move o valor do registrador $f9 para o segmento de dados da etiqueta (variável do argumento).
                    }
                }
                else if (argumentos[i].Variável.Tipo == VariávelTipo.Caracter)
                {
                    mips.SectionText.Adicionar(new MipsText("la", $"$a0,{argumentos[i].ObterEtiqueta()}"));

                    if (this.Parametros[pr].Ponteiro)
                    {
                        mips.SectionText.Adicionar(new MipsText("sw", $"$a0,{decProcSem.ObterEtiqueta()}"));
                    }
                    else
                    {
                        mips.SectionText.Adicionar(new MipsText("la", $"$a1,{decProcSem.ObterEtiqueta()}"));

                        // Inicializa o argumento contador da quantidade de caracteres.
                        mips.SectionText.Adicionar(new MipsText("li", "$a2,0"));

                        mips.SectionText.Adicionar(new MipsText("jal", "funcTextoMove")); // Adiciona a chamada da função.
                    }
                }

                id++;
                if (id >= this.Parametros[pr].Identificadores.Length)
                {
                    pr++;
                    id = 0;
                }
            }

            mips.HandlerStackPointer.IncrementaContexto();

            mips.SectionText.Adicionar(new MipsText("jal", $"{this.Nome.Lexema}")); // Adiciona a chamada da função.
            
            // Se existirem parâmetros por ponteiro, transfere os valores para as variáveis de argumento.
            if (prFuncPonteiro.Count > 0)
                mips.SectionText.Adicionar(prFuncPonteiro);

            if (prFuncPonteiroLog.Count > 0)
            {
                for (int i = 0; i < prFuncPonteiroLog.Count; i++)
                {
                    mips.HandlerLogico.ObterVariável("$t4", prFuncPonteiroLog[i++]);
                    mips.HandlerLogico.SetarVariável("$t4", prFuncPonteiroLog[i]);
                }
            }

            if (prProcPonteiroCar.Count > 0)
                mips.SectionText.Adicionar(prProcPonteiroCar);

            mips.HandlerStackPointer.DecrementaContexto();
        }
        
        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar os nós subsequentes.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó da Função
            TreeNode node = treeNode.Nodes.Add(this.ToString());

            if (this.Variáveis != null)
            {
                TreeNode varLocal = node.Nodes.Add("Variáveis Locais");
                foreach (var item in this.Variáveis)
                    varLocal.Nodes.Add(item.ToString());
            }

            if (this.Comandos != null)
            {
                foreach (var comando in this.Comandos)
                    comando.GerarArvore(node);
            }

            this.Retorne.GerarArvore(node);
        }

        #endregion

        /// <summary>
        /// Retorna um texto com a informação básica da função, para utilizar nos comantários inseridos no código Assembly MPIS.
        /// </summary>
        public string InfoDeclaração()
        {
            string tmp = String.Empty;
            if (this.Parametros != null)
            {
                for (int i = 0; i < this.Parametros.Length; i++)
                {
                    tmp += this.Parametros[i].ToString();
                    if (i < this.Parametros.Length - 1)
                        tmp += "; ";
                }
            }

            return $"{this.Nome.Lexema} ({tmp}): {this.TipoRetorno.Lexema}";
        }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            int parametros = 0;
            string tmp = String.Empty;
            if (this.Parametros != null)
            {
                for (int i = 0; i < this.Parametros.Length; i++)
                {
                    parametros += this.Parametros[i].Identificadores.Length;
                    tmp += this.Parametros[i].ToString();
                    if (i < this.Parametros.Length - 1)
                        tmp += "; ";
                }
            }

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

            return $"Função: '{this.Nome.Lexema} ({tmp}): {this.TipoRetorno.Lexema}', {parametros} parâmetro, {var} variável local ({dec} declaração), {(this.Comandos == null ? 0 : this.Comandos.Count())} comandos";
        }
    }
}
